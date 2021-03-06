'use strict';

var gulp = require('gulp'),
    gutil = require('gulp-util'),
    jade = require('gulp-jade'),
    less = require('gulp-less'),
    path = require('path'),
    yargs = require('yargs');
		
		
var args   = yargs.argv;

var targetDir = args.target || '../CompiledUi/Ui';

gulp.task('templates', function() {
  return gulp.src('./templates/pages/*.jade')
      .pipe(jade({ pretty: true }))
      .pipe(gulp.dest(targetDir + '/views'));
});

gulp.task('less', function () {
  gulp.src('./style/style.less')
    .pipe(less({
      paths: [ path.join(__dirname, 'less', 'includes') ]
    }))
    .pipe(gulp.dest(targetDir + '/style'));
});

gulp.task('content', function () {
    return gulp.src('Content/**')
      .pipe(gulp.dest(targetDir + '/content'));
});

gulp.task('js', function() {
    return gulp.src('Scripts/*.js')
        .pipe(gulp.dest(targetDir + '/scripts'));
});

gulp.task('userScripts', function () {
    return gulp.src('userScripts/*.js')
        .pipe(gulp.dest(targetDir + '/scripts'));
});

gulp.task('watch', function () {
  gulp.watch('style/**/*.less', ['less']);
  gulp.watch('Scripts/*.js', ['js']);
  gulp.watch('UserScripts/*.js', ['userScripts']);
  gulp.watch('templates/**/*.jade', ['templates']);
  gulp.watch('images/**/*', ['images']);
});

// Default Task
gulp.task('build', ['templates', 'less', 'content', 'js', 'userScripts']);
gulp.task('default', ['build', 'watch']);
