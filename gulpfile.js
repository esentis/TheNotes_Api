/// <binding ProjectOpened='watch' />
"use strict";
var webroot = "./wwwroot/";
var gulp = require('gulp');
var sass = require('gulp-sass');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var rename = require('gulp-rename');
var del = require('del');

var paths = {
	scss: webroot + "lib/sass/*.{sc,sa,c}ss",
	js: webroot + "lib/js/*.js",
	cssDest: webroot + "assets/css/",
	jsDest: webroot + "assets/js/"
};

gulp.task('compile:sass', function () {
	return gulp.src(paths.scss)
		.pipe(sass({ outputStyle: 'expanded' }).on('error', sass.logError))
		.pipe(gulp.dest(paths.cssDest))
		.pipe(sass({ outputStyle: 'compressed', sourceMap: 'true' }).on('error', sass.logError))
		.pipe(rename({ suffix: '.min' }))
		.pipe(sourcemaps.write('.', { includeContent: false }))
		.pipe(gulp.dest(paths.cssDest));
});

gulp.task('compile:js', function () {
	return gulp.src(paths.js)
		.pipe(gulp.dest(paths.jsDest))
		.pipe(sourcemaps.init())
		.pipe(uglify())
		.pipe(rename({ suffix: '.min' }))
		.pipe(sourcemaps.write('.', { includeContent: false }))
		.pipe(gulp.dest(paths.jsDest));
});

gulp.task('compile', gulp.parallel('compile:sass', 'compile:js'));

gulp.task('clean:css', function () {
	return del(paths.cssDest + "/*.{css,map}");
});

gulp.task('clean:js', function () {
	return del(paths.jsDest + "/*.{js,map}");
});

gulp.task('clean', gulp.parallel('clean:css', 'clean:js'));

gulp.task('watch', function () {
	gulp.watch(paths.scss).on('change', gulp.task('compile:sass'));
	gulp.watch(paths.js).on('change', gulp.task('compile:js'));
});

gulp.task('default', gulp.series('clean', gulp.parallel('compile:sass', 'compile:js'), 'watch'));
