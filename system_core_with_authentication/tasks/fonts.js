var gulp = require("gulp")

const fonts = {
  'font-awesome': '~/node_modules/font-awesome/fonts/*',
  'ionicons': '~/node_modules/ionicons/dist/fonts/*'
}

//gulp.task('fonts', ['fonts:font-awesome']);
gulp.task('build:fonts', ['build:fonts:font-awesome', 'build:fonts:ionicons']);

//gulp.task('fonts:font-awesome', function () {
//  return gulp.src(fonts['font-awesome'])
//    //.pipe(gulp.dest('temp/html/assets/fonts'))
//    //.pipe(gulp.dest('temp/angularjs/assets/fonts'))
//    .pipe(gulp.dest('temp/assets/fonts'))
//});

gulp.task('build:fonts:font-awesome', function () {
  return gulp.src(fonts['font-awesome'])
    //.pipe(gulp.dest('dist/html/assets/fonts'))
    //.pipe(gulp.dest('dist/angularjs/assets/fonts'))
    .pipe(gulp.dest('/wwwroot/fonts'))
});


//gulp.task('fonts:ionicons', function () {
//  return gulp.src(fonts['ionicons'])
//    .pipe(gulp.dest('temp/assets/fonts/ionicons'))
//});

gulp.task('build:fonts:ionicons', function () {
    return gulp.src(fonts['ionicons'])
        .pipe(gulp.dest('/wwwroot/fonts/ionicons'))
});