<template>
    <div>
        <school-autocomplete v-bind:value.sync="selectedSchool" v-bind:default-value="searchRequest ? searchRequest.schoolId : null"></school-autocomplete>
        <sport-list input-id="gameSearchSport"
                    v-bind:enable-multiple="true"
                    v-bind:value.sync="selectedSports"
                    v-bind:default-value="searchRequest ? searchRequest.sports : null"
                    v-on:change="selectedSports = $event"></sport-list>
        <schedule-year-list input-id="gameSearchScheduleYear"
                            v-bind:value.sync="selectedScheduleYear"
                            v-bind:default-value="searchRequest ? searchRequest.scheduleYearId : activeScheduleYearId ? activeScheduleYearId : null"
                            v-on:change="selectedScheduleYear = $event"></schedule-year-list>
        <div class="row pt-2">
            <div class="col">
                <button class="btn btn-primary btn-lg" v-on:click.prevent="gotoTeams">Search</button>
            </div>
        </div>
    </div>
</template>

<script>
    import InputAutocomplete from "../base/input-autocomplete/input-autocomplete.vue";
    import InputSelect from "../base/input-select/input-select.vue";
    import ScheduleYearList from "../select-lists/schedule-year-list.vue";
    import SportList from "../select-lists/sport-list.vue";
    import SchoolAutocomplete from "../select-lists/school-autocomplete.vue";

    export default {
        name: "teams-search",
        components: {
            "input-autocomplete": InputAutocomplete,
            "input-select": InputSelect,
            "schedule-year-list": ScheduleYearList,
            "sport-list": SportList,
            "school-autocomplete": SchoolAutocomplete,
        },
        props: {
            searchRequest: {
                type: [Object, Array],
                default: null,
            },
            searchObject: {
                type: [Object],
                default: null
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
                selectedScheduleYear: this.searchRequest ? { id: this.searchRequest.scheduleYearId } : this.activeScheduleYearId ? { id: this.activeScheduleYearId } : null,
                teamId: 0
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
            hasSelection() {
                return this.selectedSports.length > 0;
            }
        },
        methods: {
             gotoTeams() {
                var oRequest = {
                    SchoolId: this.schoolId,
                    ScheduleYearId: this.scheduleYearId,
                    Sports: this.selectedSports.map(a => a.id)
                };
                window.location = "/Teams/Search?q=" + JSON.stringify(oRequest);
             }
        }
    };
</script>
