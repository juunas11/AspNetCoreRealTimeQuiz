(function () {
    "use strict";

    var app = angular.module("Quiz");

    app.controller("QuizController",
        function ($scope, Hub, $http, $rootScope, $interval, $routeParams) {
            $scope.question = null;
            $scope.waitingForQuestion = true;
            $scope.quizOver = false;

            var timeUpdater;
            var updateTimeLeftSeconds = function () {
                if ($scope.question) {
                    var secs = $scope.question.timeLeftSeconds;
                    if (secs > 0) {
                        secs = secs - 1;
                        $scope.question.timeLeftSeconds = secs;
                    }
                }
            };
            var stopCurrentUpdater = function () {
                if (angular.isDefined(timeUpdater)) {
                    $interval.cancel(timeUpdater);
                    timeUpdater = undefined;
                }
            };

            var quizId = $routeParams.quizId;
            var hub = new Hub("QuizHub", {
                listeners: {
                    "changeQuestion": function(question) {
                        console.log(question);
                        $scope.question = question;
                        $scope.waitingForQuestion = false;
                        stopCurrentUpdater();
                        timeUpdater = $interval(updateTimeLeftSeconds, 1000);
                        $rootScope.$apply();
                    },
                    "quizOver":function(data) {
                        console.log(data);
                        $scope.quizOver = true;
                        $scope.question = null;
                        //Get the results
                        $http.get("/api/quiz/" + quizId + "/results").then(
                            function (res) {
                                $scope.results = res.data;
                                //$rootScope.$apply();
                            },
                            function(res) {
                                console.error(res);
                            });
                    }
                },
                errorHandler: function(error) {
                    console.error(error);
                },
                methods:["subscribe"],
                stateChanged: function (state) {
                    switch (state.newState) {
                        case $.signalR.connectionState.connecting:
                            break;
                        case $.signalR.connectionState.connected:
                            hub.subscribe(quizId);
                            break;
                        case $.signalR.connectionState.reconnecting:
                            break;
                        case $.signalR.connectionState.disconnected:
                            break;
                    }
                }
            });

            $scope.$on("$destroy", function () {
                stopCurrentUpdater();
            });

            $http.get("/api/quiz/" + quizId + "/status").then(
                function (res) {
                    console.log(res);
                    var model = res.data;
                    if (model.status !== "Stopped") {
                        $scope.question = model.currentQuestion;
                        $scope.waitingForQuestion = false;
                        stopCurrentUpdater();
                        timeUpdater = $interval(updateTimeLeftSeconds, 1000);

                        //$rootScope.$apply();
                    }
                },
                function(res) {
                    console.error(res);
                });

            $scope.submitAnswer = function() {
                var choice = $scope.selectedChoice;
                if (choice) {
                    $http.post("/api/quiz/" + quizId + "/submitAnswer", [choice]).then(
                        function(res) {
                            console.log(res);
                        },
                        function(res) {
                            console.error(res);
                        });
                }
            };
        });
}());