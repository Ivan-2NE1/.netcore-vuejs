<template>
    <div>
        <div class="row">
            <div class="col-sm-6">
                <school-autocomplete v-bind:value.sync="selectedSchool" v-bind:default-value="searchRequest ? searchRequest.schoolId : null"></school-autocomplete>
            </div>
            <div class="col-sm-6">
                <sport-list input-id="gameSearchSport"
                            v-bind:enable-multiple="true"
                            v-bind:value.sync="selectedSports"
                            v-bind:default-value="searchRequest ? searchRequest.sports : null"
                            v-on:change="selectedSports = $event"></sport-list>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <county-list input-id="gameSearchCounties"
                             v-bind:value.sync="selectedCounty"
                             v-bind:enable-multiple="true"
                             v-bind:default-value="searchRequest ? searchRequest.counties : null"
                             v-on:change="selectedCounty = $event"></county-list>
            </div>
            <div class="col-sm-6">
                <conference-list input-id="gameSearchConferences"
                                 v-bind:value.sync="selectedConference"
                                 v-bind:enable-multiple="true"
                                 v-bind:default-value="searchRequest ? searchRequest.conferences : null"
                                 v-on:change="selectedConference = $event"></conference-list>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <schedule-year-list input-id="gameSearchScheduleYear" 
                                    v-bind:value.sync="selectedScheduleYear" 
                                    v-bind:default-value="searchRequest ? searchRequest.scheduleYearId : activeScheduleYearId ? activeScheduleYearId : null"
                                    v-on:change="selectedScheduleYear = $event"></schedule-year-list>
            </div>
            <div class="col-sm-6">
                <input-calendar input-id="gameSearchGameDate" 
                                v-bind:default-value="searchRequest ? searchRequest.gameDate : null" 
                                label="Game Date" 
                                v-on:input="selectedGameDate = $event"></input-calendar>
            </div>
        </div>
        <div class="row pt-2">
            <div class="col">
                <button class="btn btn-primary btn-lg" v-on:click.prevent="gotoGameReportSearch">Search</button>
            </div>
        </div>
    </div>
</template>

<script>
    import SchoolAutocomplete from "../select-lists/school-autocomplete.vue";
    import InputCalendar from "../base/input-calendar/input-calendar.vue";
    import ScheduleYearList from "../select-lists/schedule-year-list.vue";
    import SportList from "../select-lists/sport-list.vue";
    import CountyList from "../select-lists/county-list.vue";
    import ConferenceList from "../select-lists/conference-list.vue";

    export default {
        name: "game-search",
        components: {
            "school-autocomplete": SchoolAutocomplete,
            "input-calendar": InputCalendar,
            "sport-list": SportList,
            "schedule-year-list": ScheduleYearList,
            "county-list": CountyList,
            "conference-list": ConferenceList,
        },
        props: {
            searchRequest: {
                type: Object,
                default: null,
            },
            activeScheduleYearId: {
                type: Number,
                default: null,
            }
        },
        data() {
            return {
                selectedSchool: null,
                selectedSports: [],
                selectedCounty: [],
                selectedConference: [],
                selectedScheduleYear: null,
                selectedGameDate: null,
            }
        },
        computed: {
            schoolId() {
                if (this.selectedSchool !== null) {
                    return this.selectedSchool.id;
                }
                return null;
            },
            scheduleYearId() {
                if (this.selectedScheduleYear !== null) {
                    return this.selectedScheduleYear.id;
                }
                return null;
            },
            gameDate() {
                if (this.selectedGameDate !== null) {
                    return this.selectedGameDate;
                }
                return null;
            }
        },
        created() {
        },
        methods: {
            gotoGameReportSearch() {
                var oRequest = {
                    SchoolId: this.schoolId,
                    ScheduleYearId: this.scheduleYearId,
                    Sports: this.selectedSports.map(a => a.id),
                    Counties: this.selectedCounty.map(a => a.id),
                    Conferences: this.selectedConference.map(a => a.id),
                    GameDate: this.gameDate
                };
                window.location = "/games?q=" + JSON.stringify(oRequest);
            },
        }
    };
</script>
