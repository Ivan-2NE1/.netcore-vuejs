<template>
    <table class="table responsive">
        <caption class="d-block d-md-none">Participating Teams</caption>
        <thead>
            <tr>
                <th>Home Team</th>
                <th>Team</th>
                <th v-if="collectScore">Final Score</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(team, idx) in gameTeams">
                <td data-title="Home Team">
                    <div class="custom-control custom-radio">
                        <input type="radio"
                               v-bind:id="'homeTeamOption' + idx"
                               v-bind:key="'homeTeamOption' + idx"
                               name="HomeTeam"
                               class="custom-control-input"
                               v-bind:value="idx"
                               v-bind:checked="team.homeTeam === true"
                               v-on:change="setHomeTeam(idx)"/>
                        <label class="custom-control-label" v-bind:for="'homeTeamOption' + idx">&nbsp;</label>
                    </div>
                </td>
                <td data-title="Team">
                    <!-- Home team / reference team is always in read-only mode -->
                    <div class="form-control-static"
                         v-bind:key="'static-team' + idx"
                         v-if="(refTeamId !== null && team.teamId === refTeamId) || (refTeamId === null && team.homeTeam === true)">
                        {{ team.teamName }}
                    </div>
                    <div class="form-inline d-block" v-bind:key="'dynamic-team' + idx" v-else>
                        <team-autocomplete v-bind:label="''"
                                           v-bind:sport-id="sportId"
                                           v-bind:schedule-year-id="scheduleYearId"
                                           v-bind:default-value="team.teamId"
                                           v-on:update:value="selectTeam(idx, $event)"
                                           v-bind:key="'teamautocomplete' + idx"></team-autocomplete>
                    </div>
                </td>
                <td data-title="Final Score" v-if="collectScore">
                    <div class="form-inline">
                        <input-field v-bind:input-id="'FinalScore' + idx"
                                     v-bind:default-value="team.score"
                                     v-on:input="setTeamScore(idx, $event)"></input-field>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
    import InputField from "../../components/base/input-field/input-field.vue";
    import TeamAutocomplete from "../../components/select-lists/team-autocomplete.vue";

    export default {
        name: "participating-teams",

        components: {
            "widget-wrapper": WidgetWrapper,
            "input-field": InputField,
            "team-autocomplete": TeamAutocomplete,
        },

        props: {
            sportId: {
                type: Number,
                required: true,
            },
            scheduleYearId: {
                type: Number,
                required: true,
            },
            teams: {
                type: Array,
                default: null,
            },
            refTeamId: {
                type: Number,
                default: null,
            },
            isReport: {
                type: Boolean,
                default: true,
            },
        },

        data: function () {
            return {
                gameTeams: this.teams,
                collectScore: this.isReport,
            }
        },

        watch: {
            teams(newval) {
                this.gameTeams = newval;
            },
            isReport(newval) {
                this.collectScore = newval;
            }
        },

        methods: {
            getTeamListItem(team) {
                return { id: team.teamId, name: team.teamName };
            },

            selectTeam(idx, data) {
                var oTeam = this.gameTeams[idx];
                oTeam.teamId = data.id;
                oTeam.teamName = data.name;
                this.$emit("gameteamsupdated", this.gameTeams);
            },

            setTeamScore(idx, data) {
                var oTeam = this.gameTeams[idx];
                oTeam.score = data;
                this.$emit("gameteamsupdated", this.gameTeams);
            },

            setHomeTeam(idx) {
                for (var i = 0; i < this.gameTeams.length; i++) {
                    this.gameTeams[i].homeTeam = false;
                    if (i === idx) {
                        this.gameTeams[i].homeTeam = true;
                    }
                }
                this.$emit("gameteamsupdated", this.gameTeams);
            },
        }
    }
</script>