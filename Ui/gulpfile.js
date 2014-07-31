'use strict';

var gulp = require('gulp'),
  gutil = require('gulp-util'),
  jade = require('gulp-jade'),
	less = require('gulp-less'),
	path = require('path')

gulp.task('templates', function() {
  return gulp.src('./templates/pages/*.jade')
      .pipe(jade({ pretty: true }))
      .pipe(gulp.dest('../build/debug/views'));
});

gulp.task('less', function () {
  gulp.src('./style/style.less')
    .pipe(less({
      paths: [ path.join(__dirname, 'less', 'includes') ]
    }))
    .pipe(gulp.dest('../build/debug/style'));
});

// Default Task
gulp.task('default', ['templates', 'less']);
