// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import SchoolAutocomplete from "../components/select-lists/school-autocomplete.vue";
import SportList from "../components/select-lists/sport-list.vue";
import CountyList from "../components/select-lists/county-list.vue";
import ConferenceList from "../components/select-lists/conference-list.vue";
import PublicationList from "../components/select-lists/publication-list.vue";
import InputCalendar from "../components/base/input-calendar/input-calendar.vue";

// Give your app a unique name!
var worksheetGames = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "school-autocomplete": SchoolAutocomplete,
        "sport-list": SportList,
        "county-list": CountyList,
        "conference-list": ConferenceList,
        "publication-list": PublicationList,
        "input-calendar": InputCalendar,
    },

    data: {
        // Reactive properties
        showSearchForm: false,
        gameData: window.gameData,
        filterSport: null,
        filterSchool: null,
        filterCounty: null,
        filterConference: null,
        filterPublication: null,
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
            // after all valid filters are applied to the raw gamedata, this is what the list will show
            var oRet = this.gameData.filter(e => e.gameReportId > 0);

            if (this.filterSport !== null && this.filterSport.length > 0) {
                oRet = oRet.filter(e => this.filterSport.findIndex(s => s.id === e.sportId) >= 0);
            }

            if (this.filterSchool !== null) {
                oRet = oRet.filter(e => e.teams.findIndex(t => t.schoolId === this.filterSchool.id) >= 0);
            }

            if (this.filterCounty !== null && this.filterCounty.length > 0) {
                oRet = oRet.filter(e => this.filterCounty.findIndex(s => s.id === e.countyId) >= 0);
            }

            if (this.filterConference !== null && this.filterConference.length > 0) {
                oRet = oRet.filter(e => e.teams.findIndex(t => this.filterConference.findIndex(c => c.id === t.conference) >= 0) >= 0);
            }

            if (this.filterPublication !== null && this.filterPublication.length > 0) {
                // This is a bonkers filter to write, need to find games with teams that have an entry in their publications list that is contained in the filter publications list
                var pubs = this.filterPublication.map(({ id }) => id);
                oRet = oRet.filter(e => pubs.every(p => e.publications.includes(p)));
            }

            return oRet;
        },

        hasFilter() {
            return (this.filterSport !== null && this.filterSport.length > 0) || (this.filterSchool !== null) ||
                (this.filterCounty !== null && this.filterCounty.length > 0) || (this.filterConference !== null && this.filterConference.length > 0) ||
                (this.filterPublication !== null && this.filterPublication.length > 0);
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
            window.location = "/Worksheet/Games/" + oMoment.format("YYYY") + "/" + oMoment.format("MM") + "/" + oMoment.format("DD");
        },

        clearFilter() {
            this.filterSport = null;
            this.filterSchool = null;
            this.filterCount = null;
            this.filterConference = null;
            this.filterPublication = null;
        },
    }
});
