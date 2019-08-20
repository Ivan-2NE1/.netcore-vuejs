import VueRouter from "vue-router";
import Vuex from "vuex";
Vue.use(VueRouter);
Vue.use(Vuex);

import ChooseSport from "./components/ChooseSport.vue";
import ChooseTeam from "./components/ChooseTeam.vue";
import ChooseEventType from "./components/ChooseEventType.vue";
import ScheduleGame from "./components/ScheduleGame.vue";


const routes = [
    {
        path: "/",
        name: "index",
        component: ChooseSport,
    },
    {
        path: "/chooseTeam",
        name: "chooseTeam",
        component: ChooseTeam,
        beforeEnter: (to, from, next) => {
            if (store.state.scheduleGame.sportId <= 0) {
                next({ path: '/' });
            } else {
                next();
            }
        },
    },
    {
        path: "/chooseEventType",
        name: "chooseEventType",
        component: ChooseEventType,
        beforeEnter: (to, from, next) => {
            if (store.state.scheduleGame.sportId <= 0) {
                next({ path: '/' });
            } else if (store.state.scheduleGame.teamId <= 0) {
                next({ path: '/chooseTeam' });
            } else {
                next();
            }
        },
    },
    {
        path: "/scheduleGame",
        name: "scheduleGame",
        component: ScheduleGame,
        beforeEnter: (to, from, next) => {
            if (store.state.scheduleGame.sportId <= 0) {
                next({ path: '/' });
            } else if (store.state.scheduleGame.teamId <= 0) {
                next({ path: '/chooseTeam' });
            } else if (store.state.scheduleGame.eventTypeId <= 0) {
                next({ path: '/chooseEventType' });
            } else {
                next();
            }
        },
    },
]

const router = new VueRouter({
    mode: "history",
    base: "/Game/Schedule/",
    routes // short for `routes: routes`
})

const store = new Vuex.Store({
    state: {
        scheduleGame: window.scheduleGame,
    },
    mutations: {
        selectSport(state, newVal) {
            state.scheduleGame.sportId = newVal.id;

            if (newVal !== undefined && newVal !== null) {
                if (Array.isArray(newVal)) {
                    newVal = newVal[0];
                }

                if (newVal.id && newVal.id > 0) {
                    state.scheduleGame.sportId = newVal.id;
                }
            } else {
                state.scheduleGame.sportId = null;
            }

        },
        selectTeam(state, newVal) {
            if (newVal !== null && newVal.id && newVal.id > 0) {
                state.scheduleGame.refTeamId = newVal.id;
                state.scheduleGame.teams[0].teamId = newVal.id;
                state.scheduleGame.teams[0].teamName = newVal.name;
            }
        },

        selectEventType(state, newval) {
            if (newval !== undefined && newval !== null && newval.length > 0) {
                var val = newval[0].id.split("|");
                state.scheduleGame.eventTypeId = Number(val[0]);
                state.scheduleGame.roundId = Number(val[1]);
                state.scheduleGame.sectionId = Number(val[2]);
                state.scheduleGame.groupId = Number(val[3]);
            } else {
                state.scheduleGame.eventTypeId = 0;
                state.scheduleGame.roundId = 0;
                state.scheduleGame.sectionId = 0;
                state.scheduleGame.groupId = 0;
            }
        },

        selectGameDate(state, newval) {
            var m = moment(newval, "MM/DD/YYYY");
            var valid = m.isValid(); // false
            var refDate = new Date(state.scheduleGame.gameDate);
            if (valid) {
                refDate.setFullYear(m.year());
                refDate.setMonth(m.month());
                refDate.setDate(m.date());
            } else {
                refDate.setFullYear(1);
                refDate.setMonth(0);
                refDate.setDate(1);
            }
            state.scheduleGame.gameDate = refDate.toJSON();
        },

        selectGameTime(state, newval) {
            var m = moment(newval, "h:mm A");
            var valid = m.isValid();
            var refDate = new Date(state.scheduleGame.gameDate);
            if (valid) {
                refDate.setHours(m.hour());
                refDate.setMinutes(m.minute());
            } else {
                refDate.setHours(0);
                refDate.setMinutes(0);
            }
            state.scheduleGame.gameDate = refDate.toJSON();
        },

        selectLocationName(state, newVal) {
            state.scheduleGame.locationName = newVal;
        },

        selectGameTeams(state, newVal) {
            state.scheduleGame.teams = newVal;
        },
    },

    getters: {
        teamId: state => {
            if (state.scheduleGame.refTeamId !== null) {
                return state.scheduleGame.refTeamId;
            }
            return 0;
        },
        selectedEventTypeInfo: state => {
            return state.scheduleGame.eventTypeId + "|" + state.scheduleGame.roundId + "|" + state.scheduleGame.sectionId + "|" + state.scheduleGame.groupId;
        },
    }
})

const scheduleGameApp = new Vue({
    store,
    router,
    mounted() {
        console.log("in mounted");
        console.log(store);
        if (this.$store.state.scheduleGame.refTeamId !== null && this.$store.state.scheduleGame.refTeamId > 0) {
            this.$router.push({ name: 'scheduleGame', params: {} });
        }
    },
}).$mount('#vueApp');