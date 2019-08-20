/*************************************************************************
 * Gulp configuration file (gulp.config.js)
 *************************************************************************
 * @description
 * Configuration file used by gulpfile.js
 *
 * @returns {undefined}
 *
 * @author
 *  Misty Rae McKinley https://www.applicationx.net/
 *************************************************************************/
module.exports = function () {

    // Config Global Variables
    var assetsPath = "./src/";
    var publishPath = "./wwwroot/";

    // Config Returned Object to be used in gulpfile.js
    var config = {
        vueBasePath: "./src/app/",
        vueTargetPath: "./wwwroot/app/",
        themeSassBasePath: "./src/sa4/scss/",
        themeSassTargetPath: "./wwwroot/sa4/css/"
    };
    return config;
};