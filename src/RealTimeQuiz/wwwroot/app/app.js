(function() {
    "use strict";

    var app = angular.module("Quiz", ["ngRoute", "AdalAngular", "SignalR"]);

    app.config(function ($routeProvider, $locationProvider, adalAuthenticationServiceProvider, $httpProvider) {
        $routeProvider
            .when("/",
            {
                templateUrl: "/app/templates/index.html",
                controller: "IndexController"
            })
            .when("/quiz/:quizId",
            {
                templateUrl: "/app/templates/quiz.html",
                controller: "QuizController"
            })
            .when("/admin/quiz",
            {
                templateUrl: "/app/templates/admin/quizList.html",
                controller: "AdminQuizListController",
                requireADLogin: true
            })
            .otherwise("/");

        $locationProvider.html5Mode(true).hashPrefix("!");

        var endpoints = {
             "/api/admin": "https://joonasw.net/7cd48690-bb84-4739-9c7b-01b1dd0bb2c6"
        };
        adalAuthenticationServiceProvider.init(
        {
            // Config to specify endpoints and similar for your app
            clientId: "578e5358-0801-4d0d-a35c-88f4b1c497ea",
            endpoints: {
                "/api/admin": "https://joonasw.net/7cd48690-bb84-4739-9c7b-01b1dd0bb2c6"
            },
            anonymousEndpoints: ["/api"]
        },
        $httpProvider   // pass http provider to inject request interceptor to attach tokens
        );
        //window.Logging.log = console.log;
        //window.Logging.level = 1;
    });
}());