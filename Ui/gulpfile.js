'use strict';

var gulp = require('gulp'),
    gutil = require('gulp-util'),
    jade = require('gulp-jade'),
    less = require('gulp-less'),
    path = require('path'),
		yargs = require('yargs');
		
		
var args   = yargs.argv;

var targetDir = args.target || '../build/debug/ui/';

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

gulp.task('images', function () {
    return gulp.src('images/*')
      .pipe(gulp.dest(targetDir + '/images'));
});

gulp.task('js', function() {
    return gulp.src('javascript/*.js')
        .pipe(gulp.dest(targetDir + '/scripts'));
});

// Default Task
gulp.task('default', ['templates', 'less', 'images', 'js']);
