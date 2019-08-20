import InputField from "../../components/base/input-field/input-field.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";

import draggable from 'vuedraggable';

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var adminDashboard = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-select": InputSelect,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar,
        draggable
    },
    created() {
        this.addGameMetaForm = this.getAddGameMetaForm();
    },
    data: {
        sport: window.sport,
        addGameMetaForm: {},
        editRow: null,
        metaValueTypes: [
            'System.Boolean',
            'System.Decimal',
            'System.Int32',
            'System.Integer',
            'System.String',
            'VSAND.DistanceMeasure',
            'VSAND.GolfPlayFormat',
            'VSAND.WrestlingWeightClass'
        ]
    },
    computed: {

    },
    methods: {
        addGameMeta() {
            var self = this;

            self.addGameMetaForm.sortOrder = self.sport.gameMeta.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + sport.sportId + "/GameMeta",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addGameMetaForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data && data.success) {
                        var newGameMeta = JSON.parse(JSON.stringify(self.addGameMetaForm));
                        newGameMeta.sportGameMetaId = data.id;
                        sport.gameMeta.push(newGameMeta);

                        self.$bvToast.toast('You have added ' + self.addGameMetaForm.name, {
                            title: 'Added Game Meta',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addGameMetaForm = self.getAddGameMetaForm();
                    } else {
                        self.$bvToast.toast(data ? data.message : 'The request object was invalid.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        deleteGameMeta(gameMeta) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + gameMeta.sportId + "/GameMeta/" + gameMeta.sportGameMetaId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(gameMeta),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.gameMeta = self.sport.gameMeta.filter(function (s) {
                            return s.sportGameMetaId !== gameMeta.sportGameMetaId;
                        });

                        self.$bvToast.toast('You have deleted ' + gameMeta.name, {
                            title: 'Deleted Game Meta',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Game Meta was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddGameMetaForm() {
            return {
                name: "",
                valueType: "",
                promptHelp: "",
                sportId: this.sport.sportId
            };
        },
        saveMetaOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/GameMeta/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sport.gameMeta),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === true) {
                        self.$bvToast.toast('You have successfully saved the sort order.', {
                            title: 'Sort Order Updated',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('An error occurred. The sort order was not saved. Please refresh and try again.', {
                            title: 'Sort Order Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        startEditGameMeta(gameMeta) {
            this.editRow = JSON.parse(JSON.stringify(gameMeta));
        },
        saveGameMeta() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/GameMeta/" + self.editRow.sportGameMetaId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Game Meta Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sport.gameMeta = self.sport.gameMeta.map(gm => {
                            if (gm.sportGameMetaId === self.editRow.sportGameMetaId) {
                                return self.editRow;
                            }

                            return gm;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The game meta was not saved.', {
                            title: 'Game Meta Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        }
    }
});