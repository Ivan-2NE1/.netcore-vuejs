// 0. If using a module system (e.g. via vue-cli), import Vue and VueRouter
// and then call `Vue.use(VueRouter)`.
import VueRouter from "vue-router";
import Vuex from "vuex";
Vue.use(VueRouter);
Vue.use(Vuex);

// 1. Define route components.
// These can be imported from other files
//const Foo = { template: '<div>foo</div>' }
//const Bar = { template: '<div>bar</div>' }
import ChooseSport from "./components/ChooseSport.vue";
import ChooseTeam from "./components/ChooseTeam.vue";
import GameHistory from "./components/GameHistory.vue";
import ChooseEventType from "./components/ChooseEventType.vue";
import GameReport from "./components/GameReport.vue";

// 2. Define some routes
// Each route should map to a component. The "component" can
// either be an actual component constructor created via
// `Vue.extend()`, or just a component options object.
// We'll talk about nested routes later.
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
            if (store.state.addGame.sportId <= 0) {
                next({ path: '/' });
            } else {
                next();
            }  
        },
    },
    //{
    //    path: "/history",
    //    name: "gameHistory",
    //    component: GameHistory,
    //    beforeEnter: (to, from, next) => {
    //        if (store.state.addGame.sportId <= 0) {
    //            next({ path: '/' });
    //        } else if (store.state.addGame.teamId <= 0) {
    //            next({ path: '/chooseTeam' });
    //        } else {
    //            next();
    //        }  
    //    },
    //},
    {
        path: "/chooseEventType",
        name: "chooseEventType",
        component: ChooseEventType,
        beforeEnter: (to, from, next) => {
            if (store.state.addGame.sportId <= 0) {
                next({ path: '/' });
            } else if (store.state.addGame.teamId <= 0) {
                next({ path: '/chooseTeam' });
            } else {
                next();
            }  
        },
    },
    {
        path: "/createGame",
        name: "createGame",
        component: GameReport,
        beforeEnter: (to, from, next) => {
            if (store.state.addGame.sportId <= 0) {
                next({ path: '/' });
            } else if (store.state.addGame.teamId <= 0) {
                next({ path: '/chooseTeam' });
            } else if (store.state.addGame.eventTypeId <= 0) {
                next({ path: '/chooseEventType' });
            } else {
                next();
            }           
        },
    },
]

// 3. Create the router instance and pass the `routes` option
// You can pass in additional options here, but let's
// keep it simple for now.
const router = new VueRouter({
    mode: "history",
    base: "/Game/Report/",
    routes // short for `routes: routes`
})

const store = new Vuex.Store({
    state: {
        addGame: window.addGame,
    },
    mutations: {
        selectSport(state, newVal) {
            state.addGame.sportId = newVal.id;

            if (newVal !== undefined && newVal !== null) {
                if (Array.isArray(newVal)) {
                    newVal = newVal[0];
                }

                if (newVal.id && newVal.id > 0) {
                    state.addGame.sportId = newVal.id;
                }
            } else {
                state.addGame.sportId = null;
            }

        },
        selectTeam(state, newVal) {
            if (newVal !== null && newVal.id && newVal.id > 0) {
                state.addGame.refTeamId = newVal.id;
                state.addGame.teams[0].teamId = newVal.id;
                state.addGame.teams[0].teamName = newVal.name;
            }
        },
        selectEventType(state, newval) {
            // newval.id should be a pipe-delimited string of eventtypeid|roundid|sectionid|groupid            
            if (newval !== undefined && newval !== null && newval.length > 0) {
                var val = newval[0].id.split("|");
                state.addGame.eventTypeId = Number(val[0]);
                state.addGame.roundId = Number(val[1]);
                state.addGame.sectionId = Number(val[2]);
                state.addGame.groupId = Number(val[3]);
            } else {
                state.addGame.eventTypeId = 0;
                state.addGame.roundId = 0;
                state.addGame.sectionId = 0;
                state.addGame.groupId = 0;
            }
        },

        selectReportSource(state, newval) {
            state.addGame.source = newval;
        },

        selectGameDate(state, newval) {
            var m = moment(newval, "MM/DD/YYYY");                 
            var valid = m.isValid(); // false
            var refDate = new Date(state.addGame.gameDate);
            if (valid) {                
                refDate.setFullYear(m.year());
                refDate.setMonth(m.month());
                refDate.setDate(m.date());
            } else {
                refDate.setFullYear(1);
                refDate.setMonth(0);
                refDate.setDate(1);
            }          
            state.addGame.gameDate = refDate.toJSON();
        },

        selectGameTime(state, newval) {
            var m = moment(newval, "h:mm A");
            var valid = m.isValid();
            var refDate = new Date(state.addGame.gameDate);
            if (valid) {
                refDate.setHours(m.hour());
                refDate.setMinutes(m.minute());
            } else {
                refDate.setHours(0);
                refDate.setMinutes(0);
            }
            state.addGame.gameDate = refDate.toJSON();
        },

        selectLocationName(state, newval) {
            state.addGame.locationName = newVal;
        },

        selectLocationCity(state, newval) {
            state.addGame.locationCity = newVal;
        },

        selectLocationState(state, newval) {
            state.addGame.locationState = newVal;
        },

        selectGameMeta(state, newVal) {
            state.addGame.meta = newVal;
        },

        selectGameTeams(state, newVal) {
            state.addGame.teams = newVal;
        },

        selectGameNotes(state, newVal) {
            state.addGame.notes = newVal;
        }
    },
    getters: {
        teamId: state => {
            if (state.addGame.refTeamId !== null) {
                return state.addGame.refTeamId;
            }
            return 0;
        },

        selectedEventTypeInfo: state => {
            return state.addGame.eventTypeId + "|" + state.addGame.roundId + "|" + state.addGame.sectionId + "|" + state.addGame.groupId;
        },
    }
})


// 4. Create and mount the root instance.
// Make sure to inject the router with the router option to make the
// whole app router-aware.
const gameReportApp = new Vue({
    store,
    router,
    mounted() {
        console.log("in mounted");
        console.log(store);
        if (this.$store.state.addGame.refTeamId !== null && this.$store.state.addGame.refTeamId > 0) {
            this.$router.push({ name: 'createGame', params: {} });
        }
    },
}).$mount('#vueApp');

// Now the app has started!