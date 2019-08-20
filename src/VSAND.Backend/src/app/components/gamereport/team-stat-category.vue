<template>
    <tbody>
        <!-- The stat category header TR -->
        <tr>
            <td v-bind:colspan="teams.length + 1" data-title="Category"><strong>{{ category.name }}</strong></td>
        </tr>

        <!-- The stat rows -->
        <tr v-for="stat in category.stats">
            <td class="text-right" data-title="Stat">{{ stat.name }}</td>
            <td v-for="team in teams" v-bind:data-title="team.name">
                <div class="form-inline input-number">
                    <input-field v-bind:input-id="'teamstat' + team.gameReportTeamId + '-' + stat.sportTeamStatId"
                                 input-type="number"
                                 v-bind:default-value="getTeamStatValue(team.gameReportTeamId, stat.sportTeamStatId)"
                                 v-on:change="setTeamStatValue(team.gameReportTeamId, stat.sportTeamStatId, $event)">
                        <template slot="inputappend">
                            <audit-button v-if="adminUser"
                                          audit-table="vsand_GameReportTeamStat"
                                          v-bind:audit-id="getTeamStatId(team.gameReportTeamId, stat.sportTeamStatId)"></audit-button>
                        </template>
                    </input-field>
                </div>
            </td>
        </tr>
    </tbody>
</template>

<script>
    import InputField from "../base/input-field/input-field.vue";
    import AuditButton from "../base/audit-button.vue";
    import { ToastPlugin } from "bootstrap-vue";

    export default {
        name: "team-stat-category",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "input-field": InputField,
            "audit-button": AuditButton,
        },
        props: {
            category: {
                type: Object,
                required: true,
            },

            teams: {
                type: Array,
                required: true,
            },
            isAdmin: {
                type: Boolean,
                default: false,
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                adminUser: this.isAdmin,
            }
        },

        created() {
            var vm = this;

        },

        watch: {
            isAdmin(newVal, oldVal) {
                this.adminUser = newVal;
            },
        },

        methods: {
            getTeamStatId(gameReportTeamId, statId) {
                let id = 0;
                let oTeam = this.teams.find(t => t.gameReportTeamId === gameReportTeamId);
                if (oTeam !== undefined && oTeam !== null) {
                    let stat = oTeam.teamStats.find(ts => ts.statId === statId);
                    if (stat !== undefined && stat !== null) {
                        id = stat.teamStatId;
                    }
                }
                return id;
            },

            getTeamStatValue(gameReportTeamId, statId) {
                let statValue = 0;
                let oTeam = this.teams.find(t => t.gameReportTeamId === gameReportTeamId);
                if (oTeam !== undefined && oTeam !== null) {
                    let stat = oTeam.teamStats.find(ts => ts.statId === statId);
                    if (stat !== undefined && stat !== null) {
                        statValue = stat.statValue;
                    }
                }
                return statValue;
            },

            setTeamStatValue(gameReportTeamId, statId, value) {
                var vm = this;
                let statRequest = {
                    TeamStatId: this.getTeamStatId(gameReportTeamId, statId),
                    GameReportTeamId: gameReportTeamId,
                    StatId: statId,
                    StatValue: Number(value)
                }

                $.ajax({
                    method: "POST",
                    url: "/siteapi/games/saveteamstat",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(statRequest),
                    dataType: 'json'
                })
                    .done(function (data) {
                        if (data.success) {
                            // stat was saved, nothing to do here
                        } else {
                            self.$bvToast.toast(data.message, {
                                title: 'An Error Occurred',
                                appendToast: true,
                                solid: true,
                                noAutoHide: true,
                                variant: "danger"
                            });
                        }
                    }, 'json');
            },
        },
    };
</script>