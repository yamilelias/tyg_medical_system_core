'use strict';

var gulp = require('gulp');
var browserSync =require('browser-sync');

const SERVE_CONFIG = {
  port: 3400,
  server: {
    baseDir: "./temp/"
  },
  ui: {
    port: 3402
  }
}
const server = browserSync.create()

gulp.task('browser-sync', () => {
  server.init(SERVE_CONFIG);
});

gulp.task("refresh", (cb) => {
  return gulp.src("./wwwroot/app.js")
    .pipe(server.stream())
})