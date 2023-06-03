
var app = angular.module('app.controllers', []);
app.controller('Home.RandomPlaces', function($scope,places) {
    $scope.randomplace = places.getrandomplace();
    $scope.topdestination = places.gettopdestination();
    $scope.currentplace = places.getcurrentplace($scope.placename);
});

app.factory('places', function ($http) {
      var randomplaces;
      var topdestination;
      var currentplace;

      $http.get("").then(function(response) {
        randomplaces = response.data;
      });

      $http.get("").then(function (response) {
          topdestination = response.data;
      });
      

      return {
          getrandomplace: function () {
              return randomplaces;
          },
          gettopdestination: function () {
              return topdestination;
          },
          getcurrentplace: function (placename) {
              $http.get("").then(function (response) {
                  currentplace = response.data;
              });
              return currentplace;

          }
      };
});
