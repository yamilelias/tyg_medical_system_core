/**
 * This class is to build, include and minify angularjs files and other stuff. Use it when angular.js is going to be used in the project.
 */

//'use strict';

//var gulp = require('gulp');
//var fileinclude = require('gulp-file-include');
//var templateCache = require('gulp-angular-templatecache');
//var htmlhint = require('gulp-htmlhint');

//gulp.task('angularjs:html', () => {
//  /* Template */
//  gulp.src(['src/angularjs/**/*.html', '!src/angularjs/elements/*.html', '!src/angularjs/components/*.html', '!src/angularjs/components/index.html'])
//    .pipe(fileinclude({
//      prefix: '@@',
//      basepath: '@file'
//    }))
//    .pipe(htmlhint({
//      "doctype-first": false,
//      "attr-no-duplication": false
//    }))
//    .pipe(htmlhint.reporter())
//    .pipe(templateCache({
//      module: 'app.templates',
//      standalone: true
//    }))
//    .pipe(gulp.dest('temp/angularjs/js'))

//  /* Index */
//  return gulp.src(['src/angularjs/index.html'])
//    .pipe(fileinclude({
//      prefix: '@@',
//      basepath: '@file'
//    }))
//    .pipe(htmlhint())
//    .pipe(htmlhint.reporter())
//    .pipe(gulp.dest('temp/angularjs/'))
//});


//gulp.task('build:angularjs:html', () => {
//  /* Template */
//  gulp.src(['src/angularjs/**/*.html', '!src/angularjs/elements/*.html', '!src/angularjs/components/*.html', '!src/angularjs/components/index.html'])
//    .pipe(fileinclude({
//      prefix: '@@',
//      basepath: '@file'
//    }))
//    .pipe(htmlhint({
//      "doctype-first": false,
//      "attr-no-duplication": false
//    }))
//    .pipe(htmlhint.reporter())
//    .pipe(templateCache({
//      module: 'app.templates',
//      standalone: true
//    }))
//    .pipe(gulp.dest('dist/angularjs/js'))

//  /* Index */
//  return gulp.src(['src/angularjs/index.html'])
//    .pipe(fileinclude({
//      prefix: '@@',
//      basepath: '@file'
//    }))
//    .pipe(htmlhint())
//    .pipe(htmlhint.reporter())
//    .pipe(gulp.dest('dist/angularjs/'))
//});
