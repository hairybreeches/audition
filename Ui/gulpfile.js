'use strict';

var gulp = require('gulp'),
    gutil = require('gulp-util'),
    jade = require('gulp-jade'),
    less = require('gulp-less'),
    path = require('path'),
		yargs = require('yargs'),
		express = require('express'),		
		app = express();		
		
		
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
    return gulp.src('Scripts/*.js')
        .pipe(gulp.dest(targetDir + '/scripts'));
});

gulp.task('express', function() {

	var servePath = path.resolve(targetDir);
  app.use(express.static(servePath));
	gutil.log('serving directory: ' + servePath);	
  app.listen(1337);  
});

gulp.task('watch', function () {
  gulp.watch('style/**/*.less', ['less']);
  gulp.watch('Scripts/*.js', ['js']);
  gulp.watch('templates/**/*.jade', ['templates']);
  gulp.watch('images/**/*', ['images']);
});

// Default Task
gulp.task('build', ['templates', 'less', 'images', 'js']);
gulp.task('default', ['build', 'express', 'watch']);
