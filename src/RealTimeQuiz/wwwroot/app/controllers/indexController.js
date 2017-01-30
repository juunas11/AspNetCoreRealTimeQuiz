(function() {
    "use strict";

    var app = angular.module("Quiz");

    app.controller("IndexController",
        function ($scope, $http, adalAuthenticationService) {
            $scope.text = "Test";
            $http.get("/api/admin/quiz").then(
                function (res) {
                    console.log(res);
                    console.log(res.status);
                },
                function(res) {
                    console.log(res);
                    console.log(res.status);
                });

            $scope.login = function() {
                adalAuthenticationService.login();
            };
        });
}());