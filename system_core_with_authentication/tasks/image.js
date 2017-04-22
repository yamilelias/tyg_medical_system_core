'use strict';

var gulp = require('gulp');
var imagemin = require('gulp-imagemin');

gulp.task('image', function () {
  return gulp.src('./wwwroot/images/**/*')
    // .pipe(imagemin())
    //.pipe(gulp.dest('temp/html/assets/images'))
    //.pipe(gulp.dest('temp/angularjs/assets/images'))
    .pipe(gulp.dest('temp/assets/images'))
});

gulp.task('build:image', function () {
    return gulp.src('./wwwroot/images/**/*')
    // .pipe(imagemin())
    //.pipe(gulp.dest('dist/html/assets/images'))
    //.pipe(gulp.dest('dist/angularjs/assets/images'))
        .pipe(gulp.dest('wwwroot/images'))
});