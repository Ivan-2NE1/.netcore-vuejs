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
        this.addTeamStatCategoryForm = this.getAddTeamStatCategoryForm();
    },
    data: {
        sport: window.sport,
        addTeamStatCategoryForm: {},
        editRow: null
    },
    methods: {
        getDefaultSortStatName(teamStatCategory) {
            var stat = teamStatCategory.teamStats.find(ps => ps.sportTeamStatId === teamStatCategory.defaultSortStatId);
            if (stat !== undefined && stat !== null) {
                return stat.name;
            }
            return "";
        },

        addTeamStatCategory() {
            var self = this;

            self.addTeamStatCategoryForm.sortOrder = self.sport.teamStatCategories.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Sports/" + sport.sportId + "/TeamStatCategories",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addTeamStatCategoryForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newTeamStatCategory = JSON.parse(JSON.stringify(self.addTeamStatCategoryForm));
                        newTeamStatCategory.sportTeamStatCategoryId = data.id;
                        sport.teamStatCategories.push(newTeamStatCategory);

                        self.$bvToast.toast('You have added ' + self.addTeamStatCategoryForm.name, {
                            title: 'Added Team Stat Category',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addTeamStatCategoryForm = self.getAddTeamStatCategoryForm();
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

        deleteTeamStatCategory(teamStatCategory) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/Sports/' + teamStatCategory.sportId + "/TeamStatCategories/" + teamStatCategory.sportTeamStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(teamStatCategory),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.teamStatCategories = self.sport.teamStatCategories.filter(function (s) {
                            return s.sportTeamStatCategoryId !== teamStatCategory.sportTeamStatCategoryId;
                        });

                        self.$bvToast.toast('You have deleted ' + teamStatCategory.name, {
                            title: 'Deleted Team Stat Category',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Team Stat Category was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },

        getAddTeamStatCategoryForm() {
            return {
                name: "",
                sportId: this.sport.sportId
            };
        },

        saveTeamStatCategoryOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.sport.sportId + "/TeamStatCategories/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sport.teamStatCategories),
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

        startEditTeamStatCategory(teamStatCategory) {
            this.editRow = JSON.parse(JSON.stringify(teamStatCategory));
        },

        saveTeamStatCategory() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/Sports/" + self.editRow.sportId + "/TeamStatCategories/" + self.editRow.sportTeamStatCategoryId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editRow.name, {
                            title: 'Team Stat Category Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sport.teamStatCategories = self.sport.teamStatCategories.map(tsc => {
                            if (tsc.sportTeamStatCategoryId === self.editRow.sportTeamStatCategoryId) {
                                return self.editRow;
                            }

                            return tsc;
                        });

                        self.editRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The team stat category was not saved.', {
                            title: 'Team Stat Category Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },

        teamStatOptionsAdapter(item) {
            return {
                label: item.name,
                key: item.sportTeamStatId,
                value: item.sportTeamStatId
            };
        }
    }
});