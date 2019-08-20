<template>
    <widget-wrapper icon="users" title="Report a Game"
                    v-bind:padding="true">

        <div class="row" v-if="validationMsgs !== null && validationMsgs.length > 0">
            <div class="col">
                <div class="alert alert-danger">
                    <p class="lead">Please correct the following to continue:</p>
                    <ul>
                        <li v-for="msg in validationMsgs">{{ msg }}</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col" v-if="isAdmin">
                <vue-selectbase v-bind:input-id="reportSource"
                                label="Source"
                                choose-prompt="Choose Source"
                                v-bind:enable-multiple="false"
                                v-bind:required="true"
                                v-bind:options="sourceOptions"
                                option-label-key="name"
                                v-on:change="setReportSource($event)"></vue-selectbase>
            </div>
            <div class="col">
                <input-calendar input-id="gameDate"
                                label="Game Date"
                                v-on:input="setGameDate($event)"></input-calendar>
            </div>
            <div class="col">
                <input-time input-id="gameTime"
                            label="Game Time"
                            v-on:input="setGameTime($event)"></input-time>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <participating-teams v-bind:teams="addGame.teams"
                                     v-bind:refTeamId="addGame.refTeamId"
                                     v-bind:sport-id="addGame.sportId"
                                     v-bind:schedule-year-id="addGame.scheduleYearId"
                                     v-on:gameteamsupdated="onGameTeamsUpdated"
                                     v-bind:is-report="true"></participating-teams>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <input-field label="Game Location / Site"
                             v-bind:default-value="addGame.locationName"
                             v-bind:required="true"
                             v-on:input="setLocationName($event)"></input-field>
            </div>
            <div class="col">
                <input-field label="Game Location / City"
                             v-bind:default-value="addGame.locationCity"
                             v-bind:required="true"
                             v-on:input="setLocationCity($event)"></input-field>
            </div>
            <div class="col">
                <input-field label="Game Location / State"
                             v-bind:default-value="addGame.locationState"
                             v-bind:required="true"
                             v-on:input="setLocationState($event)"></input-field>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <input-field label="Notes"
                             v-bind:default-value="addGame.notes"
                             v-bind:required="true"
                             v-on:input="setNotes($event)"></input-field>
            </div>
        </div>

        <gamereport-meta v-if="sport !== null"
                         v-bind:sport-id="sport.sportId"
                         v-bind:sport-meta="sport.gameMeta"
                         v-bind:game-meta="addGame.meta"
                         v-on:gamemetaupdated="onGameMetaUpdated"></gamereport-meta>

        <template v-slot:footer>
            <button class="btn btn-primary btn-lg" v-on:click="submitGameReport">Continue</button>
        </template>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
    import vSelect from "../../components/base/vue-selectbase/vue-selectbase.vue";
    import InputCalendar from "../../components/base/input-calendar/input-calendar.vue";
    import InputTime from "../../components/base/input-time/input-time.vue";
    import InputField from "../../components/base/input-field/input-field.vue";
    import ParticipatingTeams from "../../components/gamereport/participating-teams.vue";
    import GameReportMeta from "../../components/gamereport/gamereport-meta.vue";
    export default {
        name: "game-report",

        components: {
            "widget-wrapper": WidgetWrapper,
            "vue-selectbase": vSelect,
            "input-calendar": InputCalendar,
            "input-time": InputTime,
            "input-field": InputField,
            "participating-teams": ParticipatingTeams,
            "gamereport-meta": GameReportMeta,
        },

        computed: {
            addGame() {
                return this.$store.state.addGame;
            },
            hasMeta() {
                return (this.sport !== null && this.sport.gameMeta !== null && this.sport.gameMeta.length > 0);
            },
        },

        data() {
            return {
                sport: null,
                sourceOptions: [
                    { id: "Phone", name: "Phone Call" },
                    { id: "Fax", name: "Faxed Report" },
                    { id: "Email", name: "Email Report" },
                    { id: "Chased", name: "Did not Report" },
                    { id: "Staffed", name: "Staffed" },
                ],
                reportSource: null,
                isAdmin: false,
                validationMsgs: [],
            }
        },

        created() {
            // load the sport + game meta configuration
            var vm = this;

            $.get("/siteapi/auth/isadmin", function (response) {
                var admin = (response !== undefined && response !== null && (response === "true" || response === true));
                vm.isAdmin = admin;
            });

            // load the list from the API endpoint
            $.get("/siteapi/sports/meta/" + this.addGame.sportId, function (data) {
                vm.sport = data;
            }, 'json');
        },

        methods: {
            resetValidation() {
                this.validationMsgs = [];
            },

            setReportSource(newVal) {
                this.resetValidation();
                let val = "";
                if (newVal != null && newVal.id) {
                    val = newVal.id;
                }
                this.$store.commit("selectReportSource", val);
            },

            setGameDate(newVal) {
                this.resetValidation();
                this.$store.commit("selectGameDate", newVal);
            },

            setGameTime(newVal) {
                this.resetValidation();
                this.$store.commit("selectGameTime", newVal);
            },

            setLocationName(newVal) {
                this.resetValidation();
                this.$store.commit("selectLocationName", newVal);
            },

            setLocationCity(newVal) {
                this.$store.commit("selectLocationCity", newVal);
            },

            setLocationState(newVal) {
                this.$store.commit("selectLocationState", newVal);
            },

            onGameMetaUpdated(newVal) {
                this.$store.commit("selectGameMeta", newVal);
            },

            onGameTeamsUpdated(newVal) {
                this.$store.commit("selectGameTeams", newVal);
            },

            setNotes(newVal) {
                this.$store.commit("selectGameNotes", newVal);
            },

            submitGameReport() {
                var msgs = [];

                // need to validate the data for the game report
                if (this.isAdmin) {
                    if (this.addGame.source === "" || this.addGame.source === "Web") {
                        msgs.push("You must choose the source for this game report");
                    }
                }

                var initDate = new moment("1/1/0001");
                var gameDate = new moment(this.addGame.gameDate);
                if (this.addGame.gameDate === null || this.addGame.gameDate === "" || gameDate.year() < 1900) {
                    msgs.push("The date of the game is required (date the game was played)");
                } else {
                    var now = new moment().endOf("day");
                    if (!gameDate.isValid) {
                        msgs.push("Could not parse the game date. Check your entry and try again.");
                    } else if(gameDate.isAfter(now)) {
                        msgs.push("A game you are reporting as final must take place either today, or in the past");
                    }
                }

                //TODO: validate opponents (must have at least one opp that can eventually resolve back to something?)

                if (msgs.length > 0) {
                    // there are errors in the game report, do not proceed
                    this.validationMsgs = msgs;
                    return;
                }

                // we are ready to submit the game report
                var self = this;
                $.ajax({
                    method: "POST",
                    url: "/SiteApi/Games",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(self.addGame),
                    dataType: "json"
                })
                    .done(function (data) {
                        if (data.success) {
                            // redirect to the main game screen (depends on the sport configuration)
                            console.log("should redirect right now to game report id " + data.id);
                            window.location = "/Game/" + data.id;
                        } else {
                            // there was an error somewhere in the process that we need to let them know about
                            console.log("there was an error");
                        }
                    }, "json");
            }
        }
    }
</script>