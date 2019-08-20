// Import any components that you need to use, and make sure to expose them in the components section, too!
import Scoring from "../components/gamereport/scoring.vue";
import TeamStats from "../components/gamereport/teamstats.vue";
import PlayerStats from "../components/gamereport/playerstats.vue";

// Give your app a unique name!
var gameOneView = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "scoring": Scoring,
        "teamstats": TeamStats,
        "playerstats": PlayerStats,
    },

    data: {
        // Reactive properties
        gameReport: window.gameReport,
    },
    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        var vm = this;
    },

    mounted() {
        // Anything that needs to happen after the DOM is ready
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
