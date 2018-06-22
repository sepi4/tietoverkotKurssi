var gulp = require('gulp');
var pug = require('gulp-pug');
var watch = require('gulp-watch');

var kansio = 'analysaattoriTehtavat/';

gulp.task('pug',function() {
  return gulp.src(kansio + '*.pug')
  .pipe(pug({
     doctype: 'html',
     pretty: true
  }))
  .pipe(gulp.dest(kansio));
 });

 gulp.task('kissa', function() {
  console.log('kissa kissa kissa');
 });



 gulp.task('watch', function() {
  gulp.watch('analysaattoriTehtavat/*.pug', ['pug']);
 });

gulp.task('default', ['pug']);