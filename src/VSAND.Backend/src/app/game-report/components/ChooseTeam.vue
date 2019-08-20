<template>
    <widget-wrapper icon="users" title="Choose a Team"
                    v-bind:padding="true">

        <team-autocomplete v-bind:value.sync="selectedTeam"
                           v-bind:default-value="teamId"
                           v-bind:sport-id="sportId"
                           v-bind:schedule-year-id="scheduleYearId"
                           v-on:update:value="setTeamId"></team-autocomplete>

        <template v-slot:footer>
            <router-link to="history" tag="button" class="btn btn-primary btn-lg" v-bind:disabled="teamId === null || teamId <= 0">
                Continue
            </router-link>
        </template>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
    import TeamAutocomplete from "../../components/select-lists/team-autocomplete.vue";

    export default {
        name: "choose-team",

        components: {
            "widget-wrapper": WidgetWrapper,
            "team-autocomplete": TeamAutocomplete,
        },

        computed: {
            teamId() {
                return this.$store.state.addGame.refTeamId;
            },

            sportId() {
                return this.$store.state.addGame.sportId;
            },

            scheduleYearId() {
                return this.$store.state.addGame.scheduleYearId;
            }
        },

        data: function () {
            return {
                selectedTeam: null,
                isFirst : true
            }
        },

        methods: {
            setTeamId(newVal) {
                this.selectedTeam = newVal;
                this.$store.commit("selectTeam", newVal);
            }
        },

        mounted() {
            var vm = this;
            document.addEventListener('keydown', function (event) {
                if (event.code == "Enter") {
                    var paths = window.location.href.split("/");
                    var lastPath = paths[paths.length - 1];
                    if (vm.isFirst && vm.isFirst == true || lastPath != "chooseTeam") {
                        vm.isFirst = false;
                        return;
                    }
                    var teamID = vm.selectedTeam;
                    if (teamID && teamID.id && teamID.id > 0) {
                        vm.$router.push({ name: 'chooseEventType', params: {} });
                    }
                }
            });
        }
    }

</script>