/// <binding ProjectOpened='watch' />
/*************************************************************************
 * Gulp file (gulpfile.js)
 *************************************************************************
 * @description
 * Gulpfile where all the Gulp tasks are created.
 * 
 * @author
 *  Misty Rae McKinley / Connor Greene https://www.applicationx.net/
 *************************************************************************/

"use strict";

/////////////////////////////////////////////////
//                  IMPORTS
/////////////////////////////////////////////////
const gulp = require("gulp"),
    mode = require('gulp-mode')(),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
    del = require("del"),
    rename = require("gulp-rename"),
    fs = require("fs"),
    path = require("path"),
    babel = require("gulp-babel"),
    webpack = require("webpack-stream"),
    sourcemaps = require("gulp-sourcemaps"),
    prefix = require('gulp-autoprefixer'),
    cssmin = require('gulp-clean-css'),
    sassGlob = require('gulp-sass-glob'),
    sass = require("gulp-sass"),
    named = require('vinyl-named');

var VueLoaderPlugin = require('vue-loader/lib/plugin');
var config = require("./gulp.config")();

var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

var sassOptions = {
    outputStyle: 'expanded'
};

/////////////////////////////////////////////////
//           GULP TASKS FOR VueJs
/////////////////////////////////////////////////
gulp.task("vuejs", gulp.series(function () {
    return gulp.src([config.vueBasePath + "**/*.js", "!**/index.js"])
        .pipe(named())
        .pipe(webpack({
            mode: mode.production() ? 'production' : 'development',
            externals: {
                vue: 'Vue',
                "@adi/gigya_js": "gigya"
            },
            module: {
                rules: [
                    {
                        test: /\.js$/,
                        // excluding some local linked packages.
                        // for normal use cases only node_modules is needed.
                        exclude: /node_modules|vue\/src|vue-router\//
                    },
                    {
                        test: /\.s[ac]ss$/,
                        loaders: ['vue-style-loader', 'css-loader', 'sass-loader']
                    },
                    {
                        test: /\.vue$/,
                        loader: 'vue-loader'/*,
                            options: {
                                loaders: {
                                    scss: 'vue-style-loader!css-loader!sass-loader', // <style lang="scss">
                                    sass: 'vue-style-loader!css-loader!sass-loader?indentedSyntax' // <style lang="sass">
                                }
                            }*/
                    }
                ]
            },
            plugins: [
                new VueLoaderPlugin()
            ]
        }))
        .pipe(babel({
            presets: ['env']
        }))
        .pipe(gulp.dest(config.vueTargetPath));
}));

gulp.task("vuejs:min", gulp.series(["vuejs"], function () {
    return gulp.src([config.vueTargetPath + "*.js", "!" + config.vueTargetPath + "*.min.js"])
        // https://github.com/mishoo/UglifyJS2/tree/master#uglify-fast-minify-mode
        // compression is turned off because it adds 2.5 minutes to the pipeline and the compression benefit is very small (~8kb)
        .pipe(uglify({ compress: false }))
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest(config.vueTargetPath));
}));

/////////////////////////////////////////////////
//           GULP TASKS FOR SASS
/////////////////////////////////////////////////
gulp.task("buildSass", gulp.series(function () {
    log("Build SASS");

    var tasks = getThemeSass().map(function (src) {
        return gulp.src(src.srcPath)
            .pipe(sourcemaps.init())
            .pipe(sassGlob())
            .pipe(sass(sassOptions))
            .pipe(prefix())
            .pipe(rename(src.name + ".css"))
            .pipe(gulp.dest(config.themeSassTargetPath))
            .pipe(cssmin())
            .pipe(rename({ suffix: '.min' }))
            .pipe(gulp.dest(config.themeSassTargetPath))
            .pipe(sourcemaps.write(config.themeSassTargetPath));
    });
    return merge(tasks);
}));

gulp.task("cleanVue", gulp.series(function () {
    // clean compiled Vue files
    return del(config.vueTargetPath + "*.js");
}));

gulp.task("cleanCss", gulp.series(function () {
    // clean compiled SASS files
    return del(config.themeSassTargetPath + "*.css");
}));

gulp.task("clean", gulp.parallel(["cleanVue", "cleanCss"]));

gulp.task("watchCss", gulp.series(function () {
    gulp.watch(config.themeSassBasePath + "**/*.*", gulp.series(["buildSass"]));
}));

gulp.task("watchVue", gulp.series(function () {
    gulp.watch(config.vueBasePath + "**/*.*", gulp.series(["vuejs"]));
}));

gulp.task("watch", gulp.parallel(["watchVue", "watchCss"]));

gulp.task("build", gulp.parallel(["vuejs:min", "buildSass"]));

function getThemeSass() {
    var sassEntries = [];
    // We search for index.js files inside basePath folder and make those as entries
    fs.readdirSync(config.themeSassBasePath).forEach(function (name) {
        var rawName = path.basename(name, ".scss");
        var indexFile = config.themeSassBasePath + rawName + ".scss";
        var targetFile = config.themeSassTargetPath + rawName + ".css";
        if (fs.existsSync(indexFile)) {
            sassEntries.push({ "name": rawName, "srcPath": indexFile, "destPath": targetFile });
        }
    });
    return sassEntries;
}


///////////////////////////////////////
//      UTILITY FUNCTIONS
///////////////////////////////////////
function log(msg) {
    if (typeof msg === "object") {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                console.info(msg[item]);
            }
        }
    } else {
        console.info(msg);
    }
}
