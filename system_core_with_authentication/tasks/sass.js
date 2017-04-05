'use strict';

import gulp from 'gulp';
import sass from 'gulp-sass';
import postcss from 'gulp-postcss';
import source from 'vinyl-source-stream';
import notify from 'gulp-notify';
import plumber from 'gulp-plumber';
import concat from 'gulp-concat';
import autoprefixer from 'autoprefixer';
import stripCssComments from 'gulp-strip-css-comments';


gulp.task("sass", ["sass:main", "sass:theme"]);

gulp.task("sass:main", () => {
    return gulp.src("./wwwroot/app.sass")
    .pipe(plumber({errorHandler: notify.onError(
      {
        title: "CSS Error: Line <%= error.line %>",
        message: "<%= error.message %>"
      })
    }))
    .pipe(sass())
    .pipe(postcss([ 
      autoprefixer({ browsers: ['last 2 versions'] })
    ]))
    .pipe(concat('main.css'))
    .pipe(stripCssComments())
    //.pipe(gulp.dest('./temp/html/assets/css/'))
    //.pipe(gulp.dest('./temp/angularjs/assets/css/'))
    .pipe(gulp.dest('./wwwroot/css/'))
});

gulp.task("sass:theme", () => {
    return gulp.src(["./wwwroot/sass/theme/*.sass", "!./wwwroot/sass/theme/mixin.sass"])
    .pipe(plumber({errorHandler: notify.onError(
      {
        title: "CSS Error: Line <%= error.line %>",
        message: "<%= error.message %>"
      })
    }))
    .pipe(sass())
    .pipe(postcss([ 
      autoprefixer({ browsers: ['last 2 versions'] })
    ]))
    .pipe(stripCssComments())
    //.pipe(gulp.dest('./temp/html/assets/css/theme'))
    //.pipe(gulp.dest('./temp/angularjs/assets/css/theme'))
    .pipe(gulp.dest('./wwwroot/css/'))
});


gulp.task("build:sass", ["build:sass:main", "build:sass:theme"]);

gulp.task("build:sass:main", () => {
  return gulp.src("./wwwroot/app.sass")
    .pipe(sass())
    .pipe(postcss([ 
      autoprefixer({ browsers: ['last 2 versions'] })
    ]))
    .pipe(concat('main.css'))
    .pipe(stripCssComments())
    //.pipe(gulp.dest('./dist/html/assets/css/'))
    //.pipe(gulp.dest('./dist/angularjs/assets/css/'))
    .pipe(gulp.dest('./wwwroot/css/'))
});

gulp.task("build:sass:theme", () => {
    return gulp.src(["./wwwroot/sass/theme/*.sass", "!./wwwroot/sass/theme/mixin.sass"])
    .pipe(sass())
    .pipe(postcss([ 
      autoprefixer({ browsers: ['last 2 versions'] })
    ]))
    .pipe(stripCssComments())
    //.pipe(gulp.dest('./dist/html/assets/css/theme'))
    //.pipe(gulp.dest('./dist/angularjs/assets/css/theme'))
    .pipe(gulp.dest('./wwwroot/css/'))
});