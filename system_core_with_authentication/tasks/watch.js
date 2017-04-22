var gulp = require('gulp');
var runSequence = require('run-sequence');

gulp.task("watch", () => {
    gulp.watch(["./wwwroot/**/*.sass"], ["sass", "refresh"])
    //gulp.watch(["./wwwroot/**/*.html",  '!./src/angularjs/**/*.html'], ["html", "refresh"])
})

//gulp.task("angularjs:watch", () => {
//  gulp.watch(['./src/angularjs/**/*.html'], ["angularjs:html", "refresh"])
//})