'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var postcss = require('gulp-postcss');
var source = require('vinyl-source-stream');
var notify = require('gulp-notify');
var plumber = require('gulp-plumber');
var concat = require('gulp-concat');
var autoprefixer = require('autoprefixer');
var stripCssComments = require( 'gulp-strip-css-comments');
var htmlhint = require("gulp-htmlhint");


gulp.task("demo", ["demo:sass", "demo:html", "demo:watch"]);
gulp.task("build:demo", ["build:demo:sass", "build:demo:html"]);

gulp.task("demo:watch", () => {
  gulp.watch(["./demo/demo.sass"], ["demo:sass", "refresh"])
  gulp.watch(["./demo/index.html"], ["demo:html", "refresh"])
})

gulp.task("demo:sass", () => {
  return demoSass("./demo/demo.sass", "./temp/")
});

gulp.task("build:demo:sass", () => {
  return demoSass("./demo/demo.sass", "./dist/")
});

gulp.task('demo:html', () => {
  return demoHtml(['demo/index.html'], 'temp')
});

gulp.task('build:demo:html', () => {
  return demoHtml(['demo/index.html'], 'dist')
});

function demoHtml(file, dest) {
  return gulp.src(file)
    .pipe(htmlhint())
    .pipe(htmlhint.reporter())
    .pipe(gulp.dest(dest))
}

function demoSass(file, dest) {
  return gulp.src(file)
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
    .pipe(gulp.dest(dest))
}