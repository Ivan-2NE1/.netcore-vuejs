<template>
    <widget-wrapper icon="users" title="Schedule a Game"
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
            <div class="col">
                <input-calendar input-id="gameDate"
                                v-bind:config="calendarConfig"
                                label="Game Date"
                                v-on:input="setGameDate($event)"></input-calendar>
            </div>
            <div class="col">
                <input-time input-id="gameTime"
                            label="Game Time (Optional)"
                            v-on:input="setGameTime($event)"></input-time>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <participating-teams v-bind:teams="scheduleGame.teams"
                                     v-bind:refTeamId="scheduleGame.refTeamId"
                                     v-bind:sport-id="scheduleGame.sportId"
                                     v-bind:schedule-year-id="scheduleGame.scheduleYearId"
                                     v-on:gameteamsupdated="onGameTeamsUpdated"
                                     v-bind:is-report="false"></participating-teams>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <input-field label="Location/Venue Name (Optional):"
                             v-bind:default-value="scheduleGame.locationName"
                             v-bind:required="true"
                             v-on:input="setLocationName($event)"></input-field>
            </div>
        </div>

        <template v-slot:footer>
            <button class="btn btn-primary btn-lg" v-on:click="submitScheduleGame">Schedule</button>
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

    import { ToastPlugin } from 'bootstrap-vue';
    Vue.use(ToastPlugin);

    export default {
        name: "schedule-game",

        components: {
            "widget-wrapper": WidgetWrapper,
            "vue-selectbase": vSelect,
            "input-calendar": InputCalendar,
            "input-time": InputTime,
            "input-field": InputField,
            "participating-teams": ParticipatingTeams,
        },

        computed: {
            scheduleGame() {
                return this.$store.state.scheduleGame;
            },
        },

        data() {
            return {
                sport: null,
                isAdmin: false,
                validationMsgs: [],
                calendarConfig: null
            }
        },

        created() {
            // load the sport + game meta configuration
            var vm = this;

            vm.calendarConfig = {
                minDate: moment(),
                format: 'L'
            };

            $.get("/siteapi/auth/isadmin", function (response) {
                var admin = (response !== undefined && response !== null && (response === "true" || response === true));
                vm.isAdmin = admin;
            });

            // load the list from the API endpoint
            //$.get("/siteapi/sports/meta/" + this.scheduleGame.sportId, function (data) {
            //    vm.sport = data;
            //}, 'json');
        },

        methods: {
            resetValidation() {
                this.validationMsgs = [];
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

            onGameTeamsUpdated(newVal) {
                this.resetValidation();
                this.$store.commit("selectGameTeams", newVal);
            },

            submitScheduleGame() {
                var msgs = [];

                var gameDate = new moment(this.scheduleGame.gameDate);
                if (this.scheduleGame.gameDate === null || this.scheduleGame.gameDate === "" || gameDate.year() < 1900) {
                    msgs.push("The date of the game is required (date the game was played)");
                } else if(!gameDate.isAfter(now)) {
                    msgs.push("Scheduling Date must be today or in the future.");
                } else {
                    var now = new moment().endOf("day");
                    if (!gameDate.isValid) {
                        msgs.push("Could not parse the game date. Check your entry and try again.");
                    }
                }

                var tlength = this.scheduleGame.teams.length;
                for (var i = 0; i < tlength; i++) {
                    if (this.scheduleGame.teams[i].homeTeam === false) {
                        if (this.scheduleGame.teams[i].teamId < 0 || this.scheduleGame.teams[i].teamName === null || this.scheduleGame.teams[i].teamName === "") {
                            msgs.push("You must select the opponent team!");
                        }
                    }
                }

                if (msgs.length > 0) {
                    // there are errors in the game report, do not proceed
                    this.validationMsgs = msgs;
                    return;
                }

                // we are ready to schedule a game
                var self = this;
                $.ajax({
                    method: "POST",
                    url: "/SiteApi/Games/ScheduleGame",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(self.scheduleGame),
                    dataType: 'json'
                })
                    .done(function (data) {
                        if (data.success) {
                            self.$bvToast.toast('You have successfully scheduled ' + data.Id, {
                                title: 'Scheduled a new Game',
                                autoHideDelay: 5000,
                                appendToast: true,
                                variant: "success"
                            });

                            if (self.scheduleGame.refTeamId !== null && self.scheduleGame.refTeamId > 0) {
                                window.location = "/teams/" + self.scheduleGame.refTeamId;
                            }
                                
                            
                        } else {
                            self.$bvToast.toast('Scheduling game was failed.', {
                                title: 'An Error Occurred',
                                appendToast: true,
                                noAutoHide: true,
                                variant: "danger"
                            });
                            console.log("there was an error");
                        }
                    }, "json");
            }
        }
    }
</script>