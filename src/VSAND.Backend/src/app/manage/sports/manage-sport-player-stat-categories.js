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
        this.addPlayerStatCategoryForm = this.getAddPlayerStatCategoryForm();
    },
    data: {
        sport: window.sport,
        addPlayerStatCategoryForm: {},
        editRow: null
    },
    methods: {
        getDefaultSortStatName(playerStatCategory) {
            var stat = playerStatCategory.playerStats.find(ps => ps.sportPlayerStatId === playerStatCategory.defaultSortStatId);
            if (stat !== undefined && stat !== null) {
                return stat.name;
            }
            return "";
        },

        addPlayerStatCategory() {
            var self = this;

            self.addPlayerStatCategoryForm.sortOrder = self.sport.playerStatCategories.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + sport.sportId + "/PlayerStatCategories",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addPlayerStatCategoryForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newPlayerStatCategory = JSON.parse(JSON.stringify(self.addPlayerStatCategoryForm));
                        newPlayerStatCategory.sportPlayerStatCategoryId = data.id;
                        sport.playerStatCategories.push(newPlayerStatCategory);

                        self.$bvToast.toast('You have added ' + self.addPlayerStatCategoryForm.name, {
                            title: 'Added Player Stat Category',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addPlayerStatCategoryForm = self.getAddPlayerStatCategoryForm();
                    } else {
                        self.$bvToast.toast(data.message, {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                }, 'json');
        },
        deletePlayerStatCategory(playerStatCategory) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + playerStatCategory.sportId + "/PlayerStatCategories/" + playerStatCategory.sportPlayerStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(playerStatCategory),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.playerStatCategories = self.sport.playerStatCategories.filter(function (s) {
                            return s.sportPlayerStatCategoryId !== playerStatCategory.sportPlayerStatCategoryId;
                        });

                        self.$bvToast.toast('You have deleted ' + playerStatCategory.name, {
                            title: 'Deleted Player Stat Category',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Player Stat Category was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        getAddPlayerStatCategoryForm() {
            return {
                name: "",
                sportId: this.sport.sportId
            };
        },
        savePlayerStatCategoryOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/PlayerStatCategories/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sport.playerStatCategories),
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
        startEditPlayerStatCategory(playerStatCategory) {
            this.editRow = JSON.parse(JSON.stringify(playerStatCategory));
        },
        savePlayerStatCategory() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/PlayerStatCategories/" + self.editRow.sportPlayerStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Player Stat Category Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sport.playerStatCategories = self.sport.playerStatCategories.map(psc => {
                            if (psc.sportPlayerStatCategoryId === self.editRow.sportPlayerStatCategoryId) {
                                return self.editRow;
                            }

                            return psc;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The player stat category was not saved.', {
                            title: 'Player Stat Category Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        playerStatOptionsAdapter(item) {
            return {
                label: item.name,
                key: item.sportPlayerStatId,
                value: item.sportPlayerStatId
            };
        }
    }
});