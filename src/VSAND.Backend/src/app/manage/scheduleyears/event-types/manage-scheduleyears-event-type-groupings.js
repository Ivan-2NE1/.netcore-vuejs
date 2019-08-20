import InputField from "../../../components/base/input-field/input-field.vue";
import InputCalendar from "../../../components/base/input-calendar/input-calendar.vue";
import WidgetWrapper from "../../../components/base/widget-wrapper/widget-wrapper.vue";

import draggable from 'vuedraggable';

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var eventTypeConfig = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-calendar": InputCalendar,
        "widget-wrapper": WidgetWrapper,
        draggable
    },
    data: {
        sections: window.sections,
        eventTypeId: window.eventTypeId,
        editSectionRow: null,
        editGroupRow: null,
        addSectionForm: null,
        addGroupForms: {}
    },
    created() {
        this.addSectionForm = this.getAddSectionForm();

        for (let i = 0; i < sections.length; i++) {
            let section = sections[i];

            var groupForm = this.getAddGroupForm(section);
            Vue.set(this.addGroupForms, section.sectionId, groupForm);
        }
    },
    methods: {
        /* BEGIN Section Methods */
        addSection() {
            var self = this;

            self.addSectionForm.sortOrder = self.sections.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/EventTypes/" + self.eventTypeId + "/Sections",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.addSectionForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newSection = JSON.parse(JSON.stringify(self.addSectionForm));
                        newSection.sectionId = data.id;

                        Vue.set(self.addGroupForms, newSection.sectionId, self.getAddGroupForm(newSection));
                        self.sections.push(newSection);

                        self.$bvToast.toast('You have added ' + self.addSectionForm.name, {
                            title: 'Added Section',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addSectionForm = self.getAddSectionForm();
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
        deleteSection(section) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/EventTypes/' + section.eventTypeId + "/Sections/" + section.sectionId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sections = self.sections.filter(function (s) {
                            return s.sectionId !== section.sectionId;
                        });

                        self.$bvToast.toast('You have deleted ' + section.name, {
                            title: 'Deleted Section',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Section was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        updateSection() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/" + self.editSectionRow.eventTypeId + "/Sections/" + self.editSectionRow.sectionId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editSectionRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editSectionRow.name, {
                            title: 'Section Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.sections = self.sections.map(s => {
                            if (s.sectionId === self.editSectionRow.sectionId) {
                                return self.editSectionRow;
                            }

                            return s;
                        });

                        self.editSectionRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The section was not saved.', {
                            title: 'Section Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        saveSectionOrder() {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/" + self.eventTypeId + "/Sections/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.sections),
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
        startEditSection(section) {
            this.editSectionRow = JSON.parse(JSON.stringify(section));
        },
        getAddSectionForm() {
            return {
                eventTypeId: this.eventTypeId,
                name: "",
                groups: []
            };
        },
        /* END Section Methods */
        /* BEGIN Group Methods */
        addGroup(section) {
            var self = this;

            var addGroupForm = this.addGroupForms[section.sectionId];
            addGroupForm.sortOrder = section.groups.length + 1;

            $.ajax({
                method: "POST",
                url: "/SiteApi/EventTypes/Sections/" + section.sectionId + "/Groups",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(addGroupForm),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success) {
                        var newGroup = JSON.parse(JSON.stringify(addGroupForm));
                        newGroup.groupId = data.id;
                        section.groups.push(newGroup);

                        self.$bvToast.toast('You have added ' + addGroupForm.name, {
                            title: 'Added Group',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addGroupForms[section.sectionId] = self.getAddGroupForm(section);
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
        deleteGroup(section, group) {
            var self = this;

            $.ajax({
                method: "DELETE",
                url: '/SiteApi/EventTypes/Sections/' + sections.sectionId + "/Groups/" + group.groupId,
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        section.groups = section.groups.filter(function (s) {
                            return s.groupId !== group.groupId;
                        });

                        self.$bvToast.toast('You have deleted ' + group.name, {
                            title: 'Deleted Group',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });
                    } else {
                        self.$bvToast.toast('The Group was not deleted.', {
                            title: 'An Error Occurred',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        updateGroup(section) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/Sections/" + self.editGroupRow.sectionId + "/Groups/" + self.editGroupRow.groupId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(self.editGroupRow),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have successfully updated ' + self.editGroupRow.name, {
                            title: 'Group Saved',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        section.groups = section.groups.map(g => {
                            if (g.groupId === self.editGroupRow.groupId) {
                                return self.editGroupRow;
                            }

                            return g;
                        });

                        self.editGroupRow = null;
                    } else {
                        self.$bvToast.toast('An error occurred. The group was not saved.', {
                            title: 'Group Save Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        },
        saveGroupOrder(section) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/SiteApi/EventTypes/Sections/" + section.sectionId + "/Groups/SortOrder/Update",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(section.groups),
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
        getAddGroupForm(section) {
            return {
                sectionId: section.sectionId,
                name: ""
            };
        },
        startEditGroup(group) {
            this.editGroupRow = JSON.parse(JSON.stringify(group));
        },
        /* END Group Methods */
    }
});