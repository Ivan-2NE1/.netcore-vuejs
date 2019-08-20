// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import SchoolAutocomplete from "../components/select-lists/school-autocomplete.vue";
import InputField from "../components/base/input-field/input-field.vue";
import InputCalendar from "../components/base/input-calendar/input-calendar.vue";

// Give your app a unique name!
var worksheetGames = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "school-autocomplete": SchoolAutocomplete,
        "input-field": InputField,
        "input-calendar": InputCalendar,
    },

    data: {
        // Reactive properties
        showSearchForm: false,
        historyData: window.historyData,
        filterKeyword: null,
        calendarDate: window.selectedDate,
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
        filteredGameData() {
            // after all valid filters are applied to the raw data, this is what the list will show            
            var oRet = this.historyData;
            return oRet;
        },

        hasFilter() {
            return (this.filterKeyword !== null && this.filterKeyword !== "");
        },
    },

    watch: {
        // Data and Computed properties to watch
    },

    methods: {
        // Do stuff!
        changeViewDate() {
            var oSel = this.calendarDate;
            var oMoment = moment(oSel, "MM/DD/YYYY");
            //moment().format('YYYY [escaped] YYYY');
            window.location = "/Worksheet/History/" + oMoment.format("YYYY") + "/" + oMoment.format("MM") + "/" + oMoment.format("DD");
        },

        clearFilter() {
            this.filterKeyword = null;
        },
    }
});
