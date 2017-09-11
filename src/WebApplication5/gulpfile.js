/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    gulpFilter = require('gulp-filter');

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.areas = "Areas";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("clean:areas", function (cb) {
    rimraf(paths.areas, cb);
});

gulp.task('copy:p7.main', function () {
    return gulp.src(['../p7.main/Views/**'])
        .pipe(gulp.dest('Views/'));
});

gulp.task('copy:p7.main:areas', function () {
    return gulp.src(['../p7.main/Areas/**', '!../p7.main/Areas/*/{Controllers,Controllers/**}'])
        .pipe(gulp.dest('Areas/'));
});

gulp.task('copy:brunch:wwwroot', function () {
    return gulp.src(['../brunch/wwwroot/brunched/**'])
        .pipe(gulp.dest('wwwroot/'));
});

gulp.task('copy:p7.main:static', function () {
    return gulp.src(['../p7.main/static/**'])
        .pipe(gulp.dest('wwwroot/static/'));
});

gulp.task('copy:p7.main:RiotApp', function () {
    return gulp.src(['../p7.main/RiotApp/build/**'])
        .pipe(gulp.dest('wwwroot/static/p7.main/RiotApp'));
});
gulp.task('copy:p7.main:MainJavascriptApp', function () {
    return gulp.src(['../p7.main/MainJavascriptApp/build/**'])
        .pipe(gulp.dest('wwwroot/static/p7.main/MainJavascriptApp'));
});

gulp.task('copy:P7.GraphQLViewer', function () {
    return gulp.src(['../P7.GraphQLViewer/static/**'])
        .pipe(gulp.dest('wwwroot/static/'));
});

gulp.task('copy:P7.GraphQLViewer:areas', function () {
    return gulp.src(['../P7.GraphQLViewer/Areas/**', '!../P7.GraphQLViewer/Areas/*/{Controllers,Controllers/**}'])
        .pipe(gulp.dest('Areas/'));
});
gulp.task('copy:p7.external.spa:areas', function () {
    return gulp.src(['../p7.external.spa/Areas/**', '!../p7.external.spa/Areas/*/{Controllers,Controllers/**}'])
        .pipe(gulp.dest('Areas/'));
});
gulp.task('copy:p7.Authorization:areas', function () {
    return gulp.src(['../p7.Authorization/Areas/**', '!../p7.Authorization/Areas/*/{Controllers,Controllers/**}'])
        .pipe(gulp.dest('Areas/'));
});

gulp.task('copy:DevAuth:areas', function () {
    return gulp.src(['../DevAuth/Areas/**', '!../DevAuth/Areas/*/{Controllers,Controllers/**}'])
        .pipe(gulp.dest('Areas/'));
});
gulp.task('watch', [
    'copy:brunch:wwwroot',
    'copy:p7.main',
    'copy:p7.main:areas',
    'copy:p7.main:static',
    'copy:p7.main:RiotApp',
    'copy:p7.main:MainJavascriptApp',
    'copy:P7.GraphQLViewer',
    'copy:P7.GraphQLViewer:areas',
    'copy:p7.external.spa:areas',
    'copy:p7.Authorization:areas',
    'copy:DevAuth:areas'
    ],
    function () {
        gulp.watch(['../brunch/wwwroot/brunched/**'], ['copy:brunch:wwwroot']);
        gulp.watch(['../p7.main/Views/**'], ['copy:p7.main']);
        gulp.watch(['../p7.main/Areas/**'], ['copy:p7.main:areas']);
        gulp.watch(['../p7.main/static/**'], ['copy:p7.main:static']);
        gulp.watch(['../p7.main/RiotApp/build/**'], ['copy:p7.main:RiotApp']);
        gulp.watch(['../p7.main/MainJavascriptApp/build/**'], ['copy:p7.main:MainJavascriptApp']);
        gulp.watch(['../P7.GraphQLViewer/Views/**'], ['copy:P7.GraphQLViewer']);
        gulp.watch(['../P7.GraphQLViewer/Areas/**'], ['copy:P7.GraphQLViewer:areas']); 
        gulp.watch(['../p7.external.spa/Areas/**'], ['copy:p7.external.spa:areas']);
        gulp.watch(['../p7.Authorization/Areas/**'], ['copy:p7.Authorization:areas']);
        gulp.watch(['../DevAuth/Areas/**'], ['copy:DevAuth:areas']);
    });

gulp.task("min", ["min:js", "min:css"]);
