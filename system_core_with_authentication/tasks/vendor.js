'use strict';

var gulp = require('gulp');
var concat = require("gulp-concat");

const jsLibs = [
  'wwwroot/lib/jquery/dist/jquery.js',
  'wwwroot/lib/bootstrap/dist/js/bootstrap.js',
  'wwwroot/lib/select2/dist/js/select2.full.js',
  'wwwroot/lib/datatables.net/js/jquery.dataTables.js',
  'wwwroot/lib/datatables.net-bs/js/dataTables.bootstrap.js',
  'wwwroot/lib/chartist/dist/chartist.js',
  'wwwroot/lib/chartist-plugin-pointlabels/dist/chartist-plugin-pointlabels.js',
  'wwwroot/lib/highlightjs/highlight.pack.js',
  'wwwroot/lib/highlightjs-line-numbers.js/dist/highlightjs-line-numbers.min.js',
  'wwwroot/lib/devbridge-autocomplete/dist/jquery.autocomplete.js',
  'node_modules/chart.js/dist/Chart.bundle.min.js',
  'wwwroot/lib/perfect-scrollbar/js/perfect-scrollbar.jquery.min.js'
]

const cssLibs = [
  'wwwroot/lib/bootstrap/dist/css/bootstrap.css',
  'wwwroot/lib/font-awesome/css/font-awesome.css',
  'wwwroot/lib/datatables.net-bs/css/dataTables.bootstrap.css',
  'wwwroot/lib/chartist/dist/chartist.css',
  'wwwroot/lib/highlightjs/styles/androidstudio.css',
  'wwwroot/lib/select2/dist/css/select2.css',
  'wwwroot/lib/perfect-scrollbar/css/perfect-scrollbar.min.css'
]

//const ngLibs = [
//  'bower_components/jquery/dist/jquery.js',
//  'bower_components/datatables.net/js/jquery.dataTables.js',
//  'bower_components/chartist/dist/chartist.js',
//  'bower_components/highlightjs/highlight.pack.js',
//  'bower_components/chartist-plugin-pointlabels/dist/chartist-plugin-pointlabels.js',
//  'bower_components/angular/angular.js',
//  'bower_components/angular-ui-router/release/angular-ui-router.js',
//  'node_modules/angular-ui-bootstrap/dist/ui-bootstrap.js',
//  'node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js',
//  'bower_components/angular-ui-select/dist/select.js',
//  'node_modules/angular-chartist.js/dist/angular-chartist.js',
//  'bower_components/angular-highlightjs/angular-highlightjs.js',
//  'bower_components/angular-datatables/dist/angular-datatables.min.js',
//  'bower_components/angular-datatables/dist/plugins/bootstrap/angular-datatables.bootstrap.min.js'
//]


//const ngCssLibs = [
//  'bower_components/bootstrap/dist/css/bootstrap.css',
//  'bower_components/font-awesome/css/font-awesome.css',
//  'bower_components/datatables.net-bs/css/dataTables.bootstrap.css',
//  'bower_components/chartist/dist/chartist.css',
//  'bower_components/highlightjs/styles/androidstudio.css',
//  'bower_components/select2/dist/css/select2.css',
//  'bower_components/angular-ui-select/dist/select.css',
//  'bower_components/perfect-scrollbar/css/perfect-scrollbar.min.css',
//  'bower_components/angular-datatables/dist/plugins/bootstrap/datatables.bootstrap.min.css'
//]

gulp.task('vendor', ['vendor:js', 'vendor:css'/*, 'vendor:angularjs', 'vendor:angularjs:css'*/]);

gulp.task('vendor:js', function () {
  return gulp.src(jsLibs)
    .pipe(concat('vendor.js'))
    .pipe(gulp.dest('temp/assets/js'))
});

gulp.task('vendor:css', function () {
  return gulp.src(cssLibs)
    .pipe(concat('vendor.css'))
    .pipe(gulp.dest('temp/assets/css'))
});

//gulp.task('vendor:angularjs', function () {
//  return gulp.src(ngLibs)
//    .pipe(concat('vendor.js'))
//    .pipe(gulp.dest('temp/angularjs/js'))
//});

//gulp.task('vendor:angularjs:css', function () {
//  return gulp.src(ngCssLibs)
//    .pipe(concat('vendor.css'))
//    .pipe(gulp.dest('temp/angularjs/assets/css'))
//});


gulp.task('build:vendor', ['build:vendor:js', 'build:vendor:css'/*, 'build:vendor:angularjs', 'build:vendor:angularjs:css'*/]);

gulp.task('build:vendor:js', function () {
  return gulp.src(jsLibs)
    .pipe(concat('vendor.js'))
    .pipe(gulp.dest('wwwroot/js'))
});

gulp.task('build:vendor:css', function () {
  return gulp.src(cssLibs)
    .pipe(concat('vendor.css'))
    .pipe(gulp.dest('wwwroot/css'))
});

//gulp.task('build:vendor:angularjs', function () {
//  return gulp.src(ngLibs)
//    .pipe(concat('vendor.js'))
//    .pipe(gulp.dest('dist/angularjs/js'))
//});

//gulp.task('build:vendor:angularjs:css', function () {
//  return gulp.src(ngCssLibs)
//    .pipe(concat('vendor.css'))
//    .pipe(gulp.dest('dist/angularjs/assets/css'))
//});