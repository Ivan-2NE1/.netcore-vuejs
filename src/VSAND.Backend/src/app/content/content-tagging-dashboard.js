import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import InputCheckbox from "../components/base/input-checkbox/input-checkbox.vue";
import InputCalendar from "../components/base/input-calendar/input-calendar.vue";

var gameDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "input-calendar": InputCalendar
    },

    data: {
        showSearchForm: false,
        searchHeadline: window.searchHeadline,
        searchStartDate: window.searchStartDate,
        searchEndDate: window.searchEndDate,
        searchPublished: window.searchPublished,
        stories: window.stories
    },

    methods: {
        doSearch() {
            var params = {
                headline: this.searchHeadline,
                startDate: this.searchStartDate,
                endDate: this.searchEndDate,
                published: this.searchPublished,
                pg: 1
            };
            var query = $.param(params);
            window.location = "/Content/Tagging?" + query;
        },

        gotopage(pageNumber) {
            var params = {
                headline: this.searchHeadline,
                startDate: this.searchStartDate,
                endDate: this.searchEndDate,
                published: this.searchPublished,
                pg: pageNumber
            };
            var query = $.param(params);
            window.location = "/Content/Tagging?" + query;
        }
    }
});
