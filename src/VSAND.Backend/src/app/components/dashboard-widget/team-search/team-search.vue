<template>
    <widget-wrapper icon="search" title="Search for Team">
        <input-autocomplete input-id="teamSearchSchool" label="School" v-bind:value.sync="selectedSchool" force-select source-key="name" async-src-url="/siteapi/schools/autocomplete?k=" async-src-item="id"></input-autocomplete>
        <sport-list input-id="teamSearchSport" data-bind:value.sync="selectedSport" v-on:change="selectedSport = $event"></sport-list>
        <schedule-year-list input-id="teamSearchScheduleYear" 
                            v-bind:default-value="activeScheduleYearId ? activeScheduleYearId : null"
                            data-bind:value.sync="selectedScheduleYear" 
                            v-on:change="selectedScheduleYear = $event"></schedule-year-list>

        <template v-slot:footer>
            <button class="btn btn-primary btn-lg" v-on:click.prevent="gotoTeam" v-bind:disabled="teamId <= 0">Go to Team</button>
        </template>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../../base/widget-wrapper/widget-wrapper.vue";
    import InputAutocomplete from "../../base/input-autocomplete/input-autocomplete.vue";
    import InputSelect from "../../base/input-select/input-select.vue";
    import ScheduleYearList from "../../select-lists/schedule-year-list.vue";
    import SportList from "../../select-lists/sport-list.vue";

    export default {
        name: "team-search",
        components: {
            "widget-wrapper": WidgetWrapper,
            "input-autocomplete": InputAutocomplete,
            "input-select": InputSelect,
            "schedule-year-list": ScheduleYearList,
            "sport-list": SportList,
        },
        props: {
            activeScheduleYearId: {
                type: Number,
                default: null,
            }
        },
        data() {
            return {
                selectedSchool: null,
                selectedSport: null,
                selectedScheduleYear: {id: this.activeScheduleYearId},
                teamId: 0
            }
        },
        computed: {
            hasSelection() {
                return this.selectedSchool !== undefined && this.selectedSport !== undefined && this.selectedScheduleYear !== undefined &&
                    this.selectedSchool !== null && this.selectedSport !== null && this.selectedScheduleYear !== null &&
                    this.selectedSchool.id && this.selectedSport.id && this.selectedScheduleYear.id;
            }
        },

        watch: {
            hasSelection(newval) {
                console.log("hasSelection changed to " + newval);
                if (newval) {
                    this.getTeamId();
                } else {
                    this.teamId = 0;
                }
            }
        },

        methods: {            
            getTeamId() {
                var vm = this;

                $.get("/siteapi/teams/getteamid",
                    { schoolId: vm.selectedSchool.id, sportId: vm.selectedSport.id, scheduleYearId: vm.selectedScheduleYear.id },
                    function (data) {
                        vm.teamId = data;
                });
            },

            gotoTeam() {
                window.location = "/teams/" + this.teamId;
            }
        }
    };
</script>
