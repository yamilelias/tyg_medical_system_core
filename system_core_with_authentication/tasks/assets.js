'use strict';

var gulp = require('gulp');

//gulp.task("assets", ["assets:flags"])

//gulp.task('assets:flags', function () {
//  return gulp.src('./node_modules/flag-icon-css/flags/**/*')
//    //.pipe(gulp.dest('temp/html/assets/css/flags'))
//    //.pipe(gulp.dest('temp/angularjs/assets/css/flags'))
//    .pipe(gulp.dest('tmp/assets/css/flags'))
//});


gulp.task("build:flags", ["build:assets:flags"])

gulp.task('build:assets:flags', function () {
  return gulp.src('./node_modules/flag-icon-css/flags/**/*')
    //.pipe(gulp.dest('dist/html/assets/css/flags'))
    //.pipe(gulp.dest('dist/angularjs/assets/css/flags'))
      .pipe(gulp.dest('/wwwroot/css/flags'))
});
