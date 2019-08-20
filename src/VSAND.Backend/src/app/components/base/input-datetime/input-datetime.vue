<template>
    <div class="row form-group">
        <div class="col-12">
            <label>{{ label }}</label>
        </div>
        <div class="col-md-6">
            <input-calendar v-bind:default-value="date"
                            v-on:input="dateChanged($event)"
                            v-bind:required="required"></input-calendar>
        </div>
        <div class="col-md-6">
            <input-time v-bind:default-value="time"
                        v-on:input="timeChanged($event)"
                        v-bind:required="required"></input-time>
        </div>
        <div class="col-12">
            <p class="text-muted">{{ formHelp }}</p>
        </div>
    </div>
</template>

<script>
    import InputCalendar from "../input-calendar/input-calendar.vue";
    import InputTime from "../input-time/input-time.vue";

    export default {
        name: "input-datetime",

        components: {
            "input-calendar": InputCalendar,
            "input-time": InputTime
        },

        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-time-' + Math.random().toString(36).substr(2, 9)
                }
            },
            label: {
                type: String,
                default: ""
            },
            defaultValue: {
                type: String,
                default: ""
            },
            required: {
                type: Boolean,
                default: false
            },
            formHelp: {
                type: String,
                default: ""
            }
        },

        data() {
            return {
                date: moment(this.defaultValue).format("MM/DD/YYYY"),
                time: moment(this.defaultValue).format("hh:mm A")
            }
        },

        methods: {
            dateChanged(newDate) {
                this.date = newDate;

                var dateMoment = moment(newDate, "MM/DD/YYYY");
                var timeMoment = moment(this.time, "hh:mm A");

                if (dateMoment.isValid() && timeMoment.isValid()) {
                    var dateTime = dateMoment.format("YYYY-MM-DD") + "T" + timeMoment.format("HH:mm:ss");

                    this.$emit("input", dateTime);
                }
            },

            timeChanged(newTime) {
                this.time = newTime;

                var dateMoment = moment(this.date, "MM/DD/YYYY");
                var timeMoment = moment(newTime, "hh:mm A");

                if (dateMoment.isValid() && timeMoment.isValid()) {
                    var dateTime = dateMoment.format("YYYY-MM-DD") + "T" + timeMoment.format("HH:mm:ss");

                    this.$emit("input", dateTime);
                }
            }
        }
    };
</script>
