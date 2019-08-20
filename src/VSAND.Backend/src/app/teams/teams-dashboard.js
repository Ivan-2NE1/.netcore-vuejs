import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import TeamsSearch from "../components/search-widget/teams-search.vue";


var teamDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "teams-search": TeamsSearch,
    },

    data: {
        teams: window.teams,
        filterOpponent: "",
        games: {},
        showSearchForm: false,
        searchObject: window.searchRequest,
    },

    created() {
    },

    mounted() {
    },

    computed: {
    },

    methods: {
        gotopage(pageNumber) {
            this.searchObject.pageNumber = pageNumber;
            window.location = "/Teams/Search?q=" + JSON.stringify(this.searchObject);
        }
    }
});