<template>
    <widget-wrapper icon="users" title="Choose a Game Type"
                    v-bind:padding="true">

        <eventtype-list v-bind:sport-id="sportId" 
                        v-bind:schedule-year-id="scheduleYearId"
                        v-bind:default-value="eventTypeId"
                        v-on:change="setEventType"></eventtype-list>

        <template v-slot:footer>
            <router-link to="createGame" tag="button" class="btn btn-primary btn-lg" v-bind:disabled="selectedEventType === null || selectedEventType.id <= 0">
                Continue
            </router-link>
        </template>
    </widget-wrapper>
</template>

<script>
    import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
    import EventTypeList from "../../components/select-lists/eventtype-list.vue";

    export default {
        name: "choose-eventtype",

        components: {
            "widget-wrapper": WidgetWrapper,
            "eventtype-list": EventTypeList,
        },

        computed: {
            sportId() {
                return this.$store.state.addGame.sportId;
            },

            scheduleYearId() {
                return this.$store.state.addGame.scheduleYearId;
            },

            eventTypeId() {
                return this.$store.getters.selectedEventTypeInfo;
            },
        },

        data: function () {
            return {
                selectedSportId: this.sportId,
                selectedScheduleYearId: this.scheduleYearId,
                selectedEventTypeId: this.eventTypeId,
                selectedEventType: null,
            }
        },

        methods: {
            setEventType(newVal) {
                this.selectedEventType = newVal;
                this.$store.commit("selectEventType", newVal);
            }
        },

        mounted() {
            var vm = this;
            document.addEventListener('keydown', function (event) {
                if (event.code == "Enter") {
                    var paths = window.location.href.split("/");
                    var lastPath = paths[paths.length - 1];
                    if (lastPath != "chooseEventType") {
                        return;
                    }
                    vm.$router.push({ name: 'createGame', params: {} });
                }
            });
        }
    }
</script>