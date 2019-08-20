<template>
    <div class="fc fc-ltr fc-bootstrap">
        <FullCalendar defaultView="dayGridMonth" :plugins="calendarPlugins" />
    </div>
</template>

<script>
    import FullCalendar from '@fullcalendar/vue'
    import dayGridPlugin from '@fullcalendar/daygrid'

    export default {
        name: "full-calendar",

        components: {
            FullCalendar // make the <FullCalendar> tag available
        },

        props: {

        },

        data() {
            return {
                calendarPlugins: [dayGridPlugin]
            }
        },

        methods: {

        }
    }

</script>

<style lang="sass">
    @import "~@fullcalendar/core/main.css";
    @import '~@fullcalendar/daygrid/main.css';
    @import '../../../sa4/custom/plugins/@fullcalendar/core-main-override.scss';
</style>