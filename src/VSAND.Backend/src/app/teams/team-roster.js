import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputSelect from "../components/base/input-select/input-select.vue";
import InputField from "../components/base/input-field/input-field.vue";
import GraduationYear from "../components/select-lists/graduationyear-list.vue";
import SchoolAutocomplete from "../components/select-lists/school-autocomplete.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var teamRoster = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-select": InputSelect,
        "input-field": InputField,
        "graduationyear-list": GraduationYear,
        "school-autocomplete": SchoolAutocomplete,
    },

    props: {
    },

    data: {
        positions: window.positions,
        rosterEntries: window.teamRoster,
        playerFoot: [],
        playerInch: [],
        mode: 'view',
        addPlayerForm: {},
    },

    watch: {
    },

    created() {
        var vm = this;
        for (var i = 0; i < vm.rosterEntries.length; i++) {
            vm.playerFoot[i] = vm.rosterEntries[i].player.height.split('-')[0];
            vm.playerInch[i] = vm.rosterEntries[i].player.height.split('-')[1];
        }
        this.addPlayerForm = this.getAddPlayerForm();
        this.addPlayerForm.schoolId = vm.rosterEntries[0].player.schoolId;
    },

    mounted() {

    },

    computed: {
    },

    methods: {
        positionOptionsAdapter(position) {
            return {
                label: position.name,
                key: "position_" + position.sportPositionId,
                value: position.sportPositionId
            };
        },

        getHeight(foot, inch) {
            return foot + "-" + inch;
        },

        getAddPlayerForm() {
            return {
                firstName: "",
                lastName: "",
                graduationYear: "",
                schoolId: null,
                height: "",
                weight: "",
                birthDate: "",
                hFoot: "",
                hInch: "",
            };
        },

        saveRoster(rosters, foot, inch) {
            var vm = this;
            rosters.player.height = vm.getHeight(foot, inch);
            $.ajax({
                method: "PUT",
                url: "/SiteApi/Teams/teamRoster/" + rosters.teamId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(rosters),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        vm.$bvToast.toast('You have updated ' + rosters.player.firstName + " " + rosters.player.lastName, {
                            title: 'Updated Roster',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        vm.$bvToast.toast(data ? data.message : 'The request body is invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },

        deleteRoster(rosters){
            var vm = this;
            $.ajax({
                method: "DELETE",
                url: "/SiteApi/Teams/teamRoster/" + rosters.rosterId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        vm.rosterEntries = vm.rosterEntries.filter(function (s) {
                            return s.rosterId !== rosters.rosterId;
                        })
                        vm.$bvToast.toast('You have deleted roster' + rosters.rosterId, {
                            title: 'Deleted Roster',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        vm.$bvToast.toast(data ? data.message : 'The request body is invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },

        createPlayer() {
            var vm = this;
            vm.addPlayerForm.height = vm.getHeight(vm.addPlayerForm.hFoot, vm.addPlayerForm.hInch);
            $.ajax({
                method: "POST",
                url: "/SiteApi/Players/",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(vm.addPlayerForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        vm.$bvToast.toast('You have added ' + vm.addPlayerForm.firstName + " " + vm.addPlayerForm.lastName, {
                            title: 'Added Player',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        vm.addPlayerForm = vm.getAddPlayerForm();
                    } else {
                        vm.$bvToast.toast(data ? data.message : 'The request body is invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        }
    }
});