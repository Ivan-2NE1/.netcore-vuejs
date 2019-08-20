// Gigya is loading from the /Views/Shared/_Layout.cshtml from the Advance CDN
//import "@adi/gigya_js";

// This needs to be imported from NPM
// Import the Gigya user data 
import { getUser } from "@adi/gigya_js/src/auth";

var hssnUserManager = (function () {
    let loggedIn = false;
    let userId = "";
    let userName = "";

    //#region utility methods
    function log(msg, extraArgs) {
        if (console && console.log) {
            if (extraArgs) {
                console.log("hssnUserManager: " + msg, extraArgs);
            } else {
                console.log("hssnUserManager: " + msg);
            }            
        }
    }
    //#endregion

    //#region gigya event hookup and handling
    if (window.Advance && window.Advance.Gigya && window.Advance.Gigya.ready) {
        log("Gigya already loaded");
        resolveGigya();
    } else {
        window.addEventListener("gigyaReady.adi", () => {
            log("Successfully resolved gigyaReady.adi event");
            resolveGigya();
        });
    }
    
    function resolveGigya() {
        // Attache event listeners
        window.Advance.Gigya.fn.subscribe("onLogin.adi", initUser, 'HSSN');
        window.Advance.Gigya.fn.subscribe("onLogout.adi", clearUser, 'HSSN');

        window.Advance.Gigya.fn.subscribe("newUserLogin.adi", newUserLogin, 'HSSN');
        window.Advance.Gigya.fn.subscribe("notLoggedIn.adi", notLoggedIn, 'HSSN');
        window.Advance.Gigya.fn.subscribe("noUser.adi ", noUser, 'HSSN');

        // Check for a user
        getUser().then(loadUser).catch((e) => {
            log("resolve: there is not a user");
        });
    }

    function clearUser() {
        // user has logged out, clear any local storage
        log("clearUser: gigya user logout");
    }

    function newUserLogin() {
        log("newUserLogin: ");
        //window.dataLayer.push({ 'event': 'adiLoggedIn' });
    }

    function notLoggedIn() {
        log("notLoggedIn: ");
    }

    function noUser() {
        log("noUser: ");
    }

    function initUser(userData) {
        if (userData === undefined || userData === null) {
            log("initUser doesn't have userData");
            getUser().then(loadUser).catch((e) => {
                log("initUser: there isn't a user");
            });
            return;
        }
        log("initUser has userData");
        loadUser(userData);
    }

    function loadUser(userData) {
        log("loadUser: user is logged in", userData);
        loggedIn = true;        
        userId = userData.UID;
        userName = userData.profile.username;
    }
    //#endregion

    //#region Authenticated Users
    /**
     * @description On favorite icon click, send new favorite data to API. Response is updated favorites object, which is persisted to local storage. 
     * @param none
     * @return none
     * */
    function toggleFavorite() {
        console.log("favorite icon clicked");
        //$.post('/user/follow/', hssnUser.postData, function (data) {
        //    hssnUser.user.schools = data.schools;
        //    hssnUser.user.teams = data.teams;
        //    hssnUser.cache();
        //    hssnUser.populateTab();
        //    // Add Tracking for Add to Favorites Event
        //    var favoritesButtonClass = hssnUser.favIcon.attr('data-cta');
        //    if (favoritesButtonClass == 'Add to Favorites') {
        //        var eventTrackingLabel = buildGATrackingEventLabel('');
        //        hssnTrack('sports', 'hssn_personalization_favoritesbutton_click_add', eventTrackingLabel);
        //    }
        //    hssnUser.starStatus();
        //}).fail(function (data) {
        //    console.log('fail: ', data);
        //});
    }

    /**
     * @description Populates the favorites menu with contents of the favorites object
     * @param none
     * @return none
     * */
    function populateFavorites() {
        var fav_schools = '',
            fav_teams = '',
            $schoolList = $('.hssn-fav__school-list'),
            $teamList = $('.hssn-fav__team-list');
        if (hssnUser.userLoggedIn === true) {
            console.log("User is logged in; populate their favorites");
            //if (hssnUser.user.schools.length > 0) {
            //    for (var i = 0; i < hssnUser.user.schools.length; i++) {
            //        fav_schools += '<a href="/school/' + hssnUser.user.schools[i].school_slug + '/" class="hssn-fav__school">' + hssnUser.user.schools[i].name + '</a>';
            //    }
            //    $schoolList.html(fav_schools);
            //}
            //if (hssnUser.user.teams.length > 0) {
            //    for (var i = 0; i < hssnUser.user.teams.length; i++) {
            //        fav_teams += '<a href="/school/' + hssnUser.user.teams[i].school_slug + '/' + hssnUser.user.teams[i].sport_slug + '/" class="hssn-fav__team">' + hssnUser.user.teams[i].name + '</a>';
            //    }
            //    $teamList.html(fav_teams);
            //}
            //if (hssnUser.user.schools.length > 5) {
            //    $('[for="fav-school-toggle"]').addClass('active');
            //}
            //if (hssnUser.user.teams.length > 5) {
            //    $('[for="fav-team-toggle"]').addClass('active');
            //}
        } else {
            console.log("User is not logged in; messages");
            //$schoolList.html('<span>Add your favorite schools by visiting their page and clicking on "Add to Favorites"</span>');
            //$teamList.html('<span>Add your favorite teams by visiting their page and clicking on "Add to Favorites"</span>');
        }
    }

    //#endregion    

    //#region public methods
    return {
        userInfo() {
            return {userId: userId, username: userName };
        },

        loggedIn() {
            return loggedIn;
        }
    }
    //#endregion
})();

//var hssnUser = hssnUser || {};

//hssnUser.init = function () {
//    this.userLoggedIn = false;
//    this.favIcon = $(".fav_icon");
//    this.populateTab();
//    this.starStatus();
//    this.add_favorites_button_tracking();
//    this.add_favorites_dropdown_tracking();

//    // remove toprail hssn personalization
//    $('#adv_user_favs').remove();

//    /* on hssnUserReady.adi */
//    $('html').on('hssnUserReady.adi', function () {
//        hssnUser.ready();
//    });
//};
//hssnUser.ready = function () {
//    console.log("user ready");
//    this.postData = this.session;
//    if (m_hssn_type !== undefined && m_hssn_id !== undefined) {
//        this.postData[m_hssn_type] = m_hssn_id;
//    } else {
//        console.log('error: missing schoo/team data');
//        this.favIcon.css('display', 'none');
//        return false;
//    }
//    this.populateTab();
//    this.starStatus();

//    // bind favorite toggle to the fav star
//    hssnUser.favIcon.on('click', function (e) {
//        if (hssnUser.userLoggedIn === false) {
//            return false;
//        } else {
//            hssnUser.favToggle();
//        }
//    });
//};

//hssnUser.hasSchoolTeam = function () {
//    if (m_hssn_type == "school" || m_hssn_type == "team") {
//        for (var i = 0; i < hssnUser.user[m_hssn_type + 's'].length; i++) {
//            if (hssnUser.user[m_hssn_type + 's'][i].id.toString() === m_hssn_id) {
//                return true;
//            }
//        }
//    }
//    return false;
//};

//// Gets the Slugs of the currently viewed team
//hssnUser.getSlugs = function () {

//    var school_slug;
//    var team_slug;
//    var sport_slug;

//    if (m_hssn_type == "team") {
//        // Find the team slug of this team
//        for (var teamIndex = 0; teamIndex < hssnUser.user.teams.length; teamIndex++) {
//            if (hssnUser.user.teams[teamIndex].id == m_hssn_id) {
//                team_slug = hssnUser.user.teams[teamIndex].school_slug;
//                sport_slug = hssnUser.user.teams[teamIndex].sport_slug;
//            }
//        }
//    } else if (m_hssn_type == "school") {

//        // Find the school slug of this school
//        for (var schoolIndex = 0; schoolIndex < hssnUser.user.schools.length; schoolIndex++) {
//            if (hssnUser.user.schools[schoolIndex].id == m_hssn_id) {
//                school_slug = hssnUser.user.schools[schoolIndex].school_slug;
//            }
//        }
//    }
//    return { "team_slug": team_slug, "sport_slug": sport_slug, "school_slug": school_slug }
//}

//// Builds a tracking label for the event label part of a Google Analytics Tracking Event
//function buildGATrackingEventLabel(initialLabel) {
//    var slugs = hssnUser.getSlugs();
//    var trackingLabel = initialLabel;

//    if (initialLabel) {
//        trackingLabel += "||";
//    } else {
//        trackingLabel = "";
//    }

//    // Depending on whether the user is looking at a school or team we attach the appropriate data
//    if (m_hssn_type == "school") {
//        trackingLabel += "school_slug:" + slugs.school_slug;
//    } else if (m_hssn_type == "team") {
//        trackingLabel += "team_slug:" + slugs.team_slug + "||" + "sport_slug:" + slugs.sport_slug;
//    }
//    return trackingLabel;
//}

//// Add HSSN Tracking to the favorites button
//hssnUser.add_favorites_button_tracking = function () {
//    hssnUser.favIcon.click(function (event) {
//        var favoritesButtonClass = $(this).attr('data-cta');
//        // Add HSSN Tracking for the remove and login see favToggle for add
//        if (favoritesButtonClass == 'Favorite') {
//            var eventTrackingLabel = buildGATrackingEventLabel('');
//            hssnTrack('sports', 'hssn_personalization_favoritesbutton_click_remove', eventTrackingLabel);
//        } else if (favoritesButtonClass == 'Login to Add to Favorites') {
//            hssnTrack('sports', 'hssn_personalization_favoritesbutton_click_login', '');
//        }
//    });
//}


//// Add HSSN Tracking to the favorites dropdown
//hssnUser.add_favorites_dropdown_tracking = function () {

//    // Add Tracking to the favorite schools list
//    $('.hssn-fav__school-list').click(function (event) {
//        hssnTrack('sports', 'hssn_myfavoritesdropdownlink_click', 'school');
//    });

//    // Add Tracking to the favorite teams list
//    $('.hssn-fav__team-list').click(function (event) {
//        hssnTrack('sports', 'hssn_myfavoritesdropdownlink_click', 'team');
//    });


//}

//hssnUser.starStatus = function () {
//    if (hssnUser.userLoggedIn === true) {
//        console.log("check for favorite status");
//        //if (hssnUser.hasSchoolTeam() == true) {
//        //    hssnUser.favIcon.addClass('active');
//        //    hssnUser.favIcon.attr('data-cta', 'Favorite');
//        //    hssnUser.favIcon.find('span').text('Remove from Favorites');
//        //} else {
//        //    hssnUser.favIcon.removeClass('active');
//        //    hssnUser.favIcon.attr('data-cta', 'Add to Favorites');
//        //    hssnUser.favIcon.find('span').text('Add to Favorites');
//        //}
//    } else {
//        console.log("you need to be logged in to add favorites");
//        //hssnUser.favIcon.removeClass('active');
//        //hssnUser.favIcon.find('span').text('Please Login');
//        //hssnUser.favIcon.attr('data-cta', 'Login to Add to Favorites');
//    }
//};
//hssnUser.favToggle = function () {
//    $.post('/user/follow/', hssnUser.postData, function (data) {
//        hssnUser.user.schools = data.schools;
//        hssnUser.user.teams = data.teams;
//        hssnUser.cache();
//        hssnUser.populateTab();
//        // Add Tracking for Add to Favorites Event
//        var favoritesButtonClass = hssnUser.favIcon.attr('data-cta');
//        if (favoritesButtonClass == 'Add to Favorites') {
//            var eventTrackingLabel = buildGATrackingEventLabel('');
//            hssnTrack('sports', 'hssn_personalization_favoritesbutton_click_add', eventTrackingLabel);
//        }
//        hssnUser.starStatus();
//    }).fail(function (data) {
//        console.log('fail: ', data);
//    });
//};
//hssnUser.populateTab = function () {
//    var fav_schools = '',
//        fav_teams = '',
//        $schoolList = $('.hssn-fav__school-list'),
//        $teamList = $('.hssn-fav__team-list');
//    if (hssnUser.userLoggedIn === true) {
//        console.log("User is logged in; populate their favorites");
//        //if (hssnUser.user.schools.length > 0) {
//        //    for (var i = 0; i < hssnUser.user.schools.length; i++) {
//        //        fav_schools += '<a href="/school/' + hssnUser.user.schools[i].school_slug + '/" class="hssn-fav__school">' + hssnUser.user.schools[i].name + '</a>';
//        //    }
//        //    $schoolList.html(fav_schools);
//        //}
//        //if (hssnUser.user.teams.length > 0) {
//        //    for (var i = 0; i < hssnUser.user.teams.length; i++) {
//        //        fav_teams += '<a href="/school/' + hssnUser.user.teams[i].school_slug + '/' + hssnUser.user.teams[i].sport_slug + '/" class="hssn-fav__team">' + hssnUser.user.teams[i].name + '</a>';
//        //    }
//        //    $teamList.html(fav_teams);
//        //}
//        //if (hssnUser.user.schools.length > 5) {
//        //    $('[for="fav-school-toggle"]').addClass('active');
//        //}
//        //if (hssnUser.user.teams.length > 5) {
//        //    $('[for="fav-team-toggle"]').addClass('active');
//        //}
//    } else {
//        console.log("User is not logged in; messages");
//        //$schoolList.html('<span>Add your favorite schools by visiting their page and clicking on "Add to Favorites"</span>');
//        //$teamList.html('<span>Add your favorite teams by visiting their page and clicking on "Add to Favorites"</span>');
//    }
//}

//hssnUser.init();

///* ***** hssn user profile ***** */
///* 	
//    hssnUser.user => returns user object
//    hssnUser.session => returns gigya session object
//*/
//hssnUser = (function () {
//    var pub = hssnUser,
//        tucms_page = !!($('#content.tucms').length),
//        login_page = !!($('#tucms-login').length);

//    if (!tucms_page || login_page) {

//        // Local Cache Timeout in hours for the cached hssn user object
//        pub.user_cache_time_out = 24;

//        /* Caches the user object and sets the expiration time, subsequent calls to getUser() will return
//        a cached version of the user object */
//        pub.cache = function (timeout) {
//            // Cache the user object
//            localStorage.setItem('cached_user_object', JSON.stringify(this.user));

//            // Set the cached timed based on the passed in
//            if (!timeout) {
//                timeout = this.user_cache_time_out
//            }
//            var current_date = new Date();
//            cache_expiration_time = current_date.setHours(current_date.getHours() + timeout)
//            localStorage.setItem('user_object_cached_time', cache_expiration_time.toString());
//        }

//        // Check if the timeout for the user object in local storage has expired
//        pub.has_expired = function () {
//            var currentDate = new Date();
//            var user_object_cached_time = this.get_cached_time();
//            return (!user_object_cached_time) || (currentDate.getTime() >= user_object_cached_time)
//        }

//        // Returns the last time this user object was cached or false if it was not
//        pub.get_cached_time = function () {
//            var user_object_cached_time;
//            // Check for local storage support and get the time that we last cached the user object
//            try {
//                user_object_cached_time = JSON.parse(localStorage.getItem('user_object_cached_time'));
//            } catch (e) {
//                user_object_cached_time = false;
//            }

//            return user_object_cached_time
//        }

//        // Clears the user cache
//        pub.clear_cache = function () {
//            // Set both the cached user object to null and the time cached to false
//            localStorage.setItem('cached_user_object', null);
//            localStorage.setItem('user_object_cached_time', false);
//        }

//        function getUser() {
//            pub.session = {
//                'UID': Advance.Gigya.auth.user.UID,
//                'UIDSignature': Advance.Gigya.auth.user.UIDSignature,
//                'signatureTimestamp': Advance.Gigya.auth.user.signatureTimestamp
//            };

//            if (pub.has_expired() || login_page) {
//                var path = login_page ? '/reporter/user/' : '/user/';
//                $.getJSON(path, pub.session, function (data) {
//                    pub.user = data;
//                    pub.cache();
//                    pub.userLoggedIn = true;
//                    $('html').trigger('hssnUserReady.adi', pub.user);
//                }).fail(function (data) {
//                    pub.user = {};
//                });
//            } else {
//                pub.user = JSON.parse(localStorage.getItem('cached_user_object'))
//                pub.userLoggedIn = true;
//                $('html').trigger('hssnUserReady.adi', pub.user);
//            }
//        }
//        pub.can_edit_game = function () {
//            if (!pub.user.is_reporter) {
//                return false;
//            }
//            if (pub.user.is_staff_reporter) {
//                return true;
//            }
//            if (!pub.user.reports_for.length) {
//                return false;
//            }
//            return !!($.grep(pub.user.reports_for, function (tid) {
//                return $.inArray(tid, game_team_ids) > -1;
//            }).length);
//        }
//        function clearUser() {
//            pub.session = {};
//            pub.user = {};
//            pub.clear_cache();
//            $('html').trigger('hssnUserLogout.adi', pub.user);
//        }
//        if (Advance.Gigya.auth) {
//            if (Advance.Gigya.auth.user.UID) {
//                getUser();
//            } else {
//                Advance.Gigya.fn.subscribe('getAccountInfo.adi', getUser, 'HSSN');
//            }
//            Advance.Gigya.fn.subscribe('onLogin.adi', getUser, 'HSSN');
//            Advance.Gigya.fn.subscribe('onLogout.adi', clearUser, 'HSSN');
//        }
//        return pub;
//    }
//}());

//class User {
//    constructor() {
//        this.cookieValue = Cookies.getJSON("hssv_user");
//        if (typeof this.cookieValue == "undefined") {
//            this.cookieValue = { "uid": "undefined" };
//        }
//    }

//    save_info(type, data) {
//        this.cookieValue[type] = data;
//        Cookies.set("hssv_user", this.cookieValue);
//    }

//    save_access(type, namekey, expiration) {
//        var expiration_plus = expiration + (60 * 60 * 24 * 1000) - (1000 * 60); //time + 1 day - 1 minute

//        if (typeof this.cookieValue[type] == "undefined") {
//            this.cookieValue[type] = {};
//        }
//        this.cookieValue[type][namekey] = expiration_plus;
//        Cookies.set("hssv_user", this.cookieValue);

//    }

//    getValue(query) {
//        return this.cookieValue[query]
//    }

//    get uid() {
//        return this.cookieValue["uid"]
//    }

//    save_uid(myCallback) {

//        var prevUid = false;
//        var that = this;

//        if (this.uid) {
//            prevUid = true;
//            myCallback(this.uid);
//        }

//        function gigya_cb(response) {
//            that.save_info("uid", response.UID);
//            if (!prevUid) {
//                myCallback(response.uid);
//            }
//        }

//        gigya.accounts.getAccountInfo({ callback: gigya_cb });

//    }


//    get school_preference() {
//        return this.cookieValue["school_preference"]
//    }

//    get accessType() {

//        if (this.granted) {
//            return "Via-" + this.granted
//        }

//        if (this.term) {
//            return this.term
//        }

//        if (this.school) {
//            return "P2P-" + this.school
//        }

//        return false
//    }

//    get has_access() {

//        if (this.accessType) {

//            return true

//        } else {

//            return false

//        }
//    }

//    get granted() {
//        return firstValidItem(this.cookieValue["granted"])
//    }

//    get term() {
//        return firstValidItem(this.cookieValue["term"])
//    }

//    get school() {

//        if (window.game_data && window.game_data.schoolHome.id) {
//            if (allValidItems(this.schools).includes(window.game_data.schoolHome.id)) {
//                return true
//            }
//        }

//        if (window.game_data && window.game_data.schoolAway.id) {
//            if (allValidItems(this.schools).includes(window.game_data.schoolAway.id)) {
//                return true
//            }
//        }

//        return false

//    }

//    get schools() {
//        return this.cookieValue["school"]
//    }

//    grant_access_for_daypass(accessKey) {

//        var daypass = atob(accessKey);
//        var d = new Date();
//        var formated_date = ("0" + (d.getMonth() + 1)).slice(-2) + "/" + ("0" + d.getDate()).slice(-2) + "/" + d.getFullYear();

//        if (daypass == formated_date) {
//            var expiration = Date.now() + (1000 * 60 * 60 * 24 * 2);
//            window.uo.save_access("granted", "Support", expiration);

//            var message = "Your pass is active and will expire in two days.";
//            show_alert(message, "success")

//        } else {
//            return false
//        }

//    }

//    grant_access_for_school(date, school) {

//        if (!date || !school) {
//            return
//        }

//        // Base64 decode
//        var d = atob(date);
//        d = new Date(Date.parse(d));

//        window.uo.save_access("school", atob(school), d.getTime());

//        var message = "Your school access pass for " + atob(school) + ' is active and will expire on ' + d.toLocaleDateString("en-US", dateoptions);
//        show_alert(message, "success")

//    }

//}

