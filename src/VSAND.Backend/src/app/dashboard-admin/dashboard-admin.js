import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import GameSearch from "../components/search-widget/game-search.vue";
import TeamSearch from "../components/dashboard-widget/team-search/team-search.vue";

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "game-search": GameSearch,
        "team-search": TeamSearch,
        "widget-wrapper": WidgetWrapper,
    },
    data: {
        
    },

    methods: {
        doGameSearch() {
            
        }
    }
});