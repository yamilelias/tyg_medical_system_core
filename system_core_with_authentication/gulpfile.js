'use strict';

var gulp = require('gulp');
var requireDir = require('require-dir');
var runSequence = require('run-sequence');

const tasks = requireDir("./tasks");

//gulp.task('angularjs', () => {
//  return runSequence('angularjs:html', 'angularjs:js:watch', 'angularjs:watch')
//})

//gulp.task('build:angularjs', () => {
//  return runSequence('build:angularjs:html', 'build:angularjs:js')
//})

//gulp.task('dev', () => {
//  return runSequence('clean', /*'html',*/ 'sass', 'fonts','image', 'assets', 'vendor', /*'demo',*/ 'js:watch','browser-sync',/*'angularjs',*/ 'watch');
//});

//gulp.task('build', () => {
//    return runSequence('build:clean', /*'build:html',*/ 'build:sass', 'build:fonts', 'build:image', 'build:assets', 'build:vendor', 'build:js', /*'build:angularjs', 'build:demo'*/);
//})

// Uncomment this and the respective classes if needed
//gulp.task('dev', () => {
//  return runSequence('clean', 'sass', 'fonts','image', 'assets', 'vendor', 'js:watch','browser-sync','watch');
//});

gulp.task('build', () => {
    return runSequence('build:clean', 'build:sass', 'build:fonts', 'build:image', 'build:flags', 'build:vendor', 'build:js');
});