// Import any components that you need to use, and make sure to expose them in the components section, too!
import PlayerStats from "../components/gamereport/playerstats.vue";

// Give your app a unique name!
var gamePlayerStats = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "playerstats": PlayerStats,
    },

    data: {
        // Reactive properties
        gameReportId: window.gameReportId,
        gameReportTeam: window.gameReportTeam,
        gameReportSport: window.gameReportSport,
        validationMsgs: [],
        isAdmin: false,
    },

    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        // load the sport + game meta configuration
        var vm = this;

        $.get("/siteapi/auth/isadmin", function (response) {
            var admin = (response !== undefined && response !== null && (response === "true" || response === true));
            vm.isAdmin = admin;
        });
    },

    mounted() {
        // Anything that needs to take place after the app is mounted
        var vm = this;

    },

    computed: {
        // Computed properties
    },

    watch: {
        // Data and Computed properties to watch
    },

    methods: {
        // Do stuff!
        
    }
});
