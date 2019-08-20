// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import GameSearch from "../components/search-widget/game-search.vue";

// Give your app a unique name!
var gameDashboard = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "game-search": GameSearch,
    },

    data: {
        // Reactive properties
        showSearchForm: false,
        searchObject: window.searchRequest,
    },
    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        var vm = this;
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
        gotopage(pageNumber) {
            this.searchObject.pageNumber = pageNumber;
            window.location = "/games?q=" + JSON.stringify(this.searchObject);
        }
    }
});
