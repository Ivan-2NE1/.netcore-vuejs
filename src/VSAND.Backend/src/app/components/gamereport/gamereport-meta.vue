<template>
    <div v-if="sportMeta !== null || sportMeta.length <= 0">
        <div class="row" v-for="meta in sportMeta">
            <div class="col">
                <div class="form-inline" v-if="meta.valueType === 'System.Int32'" v-bind:key="'metainput-' + meta.sportGameMetaId">
                    <input-field v-bind:default-value="getMetaValue(meta.sportGameMetaId)"
                                 type="number"
                                 v-bind:label="meta.name"
                                 v-bind:form-help="meta.promptHelp"
                                 v-on:input="setMetaValue(meta.sportGameMetaId, $event)"></input-field>
                </div>
                <div v-else-if="meta.valueType === 'VSAND.DistanceMeasure'" v-bind:key="'metainput-' + meta.sportGameMetaId">
                    <event-distance-measure v-bind:label="meta.name"
                                            v-bind:form-help="meta.promptHelp"
                                            v-bind:default-value="getMetaValue(meta.sportGameMetaId)"
                                            v-on:change="setMetaValue(meta.sportGameMetaId, $event ? $event.id : $event)"></event-distance-measure>
                </div>
                <div v-else-if="meta.valueType === 'VSAND.GolfPlayFormat'" v-bind:key="'metainput-' + meta.sportGameMetaId">
                    <golf-play-format v-bind:label="meta.name"
                                            v-bind:form-help="meta.promptHelp"
                                            v-bind:default-value="getMetaValue(meta.sportGameMetaId)"
                                            v-on:change="setMetaValue(meta.sportGameMetaId, $event ? $event.id : $event)"></golf-play-format>
                </div>
                <div v-else-if="meta.valueType === 'VSAND.WrestlingWeightClass'" v-bind:key="'metainput-' + meta.sportGameMetaId">
                    <wrestling-weight-class v-bind:sport-id="sportId" v-bind:label="meta.name" 
                                            v-bind:form-help="meta.promptHelp" 
                                            v-bind:default-value="getMetaValue(meta.sportGameMetaId)" 
                                            v-on:change="setMetaValue(meta.sportGameMetaId, $event ? $event.id : $event)"></wrestling-weight-class>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import InputField from "../../components/base/input-field/input-field.vue";
    import InputSwitch from "../../components/base/input-switch.vue";
    import VueSelectBase from "../../components/base/vue-selectbase/vue-selectbase.vue";
    import WrestlingWeightClass from "../../components/gamereport/wrestling-weight-class.vue";
    import EventDistanceMeasure from "../../components/gamereport/event-distance-measure.vue";
    import GolfPlayFormat from "../../components/gamereport/golf-play-format.vue";

    export default {
        name: "gamereport-meta",

        components: {
            "input-field": InputField,
            "input-switch": InputSwitch,
            "vue-selectbase": VueSelectBase,
            "wrestling-weight-class": WrestlingWeightClass,
            "event-distance-measure": EventDistanceMeasure,
            "golf-play-format": GolfPlayFormat,
        },

        props: {
            sportId: {
                type: Number,
                required: true,
            },
            sportMeta: {
                type: Array,
                default: null,
            },
            gameMeta: {
                type: Array,
                default: null,
            }
        },

        data: function () {
            return {
                gameReportMeta: this.gameMeta,
            }
        },

        watch: {
            gameMeta(newval) {
                this.gameReportMeta = newval;
            }
        },

        created() {
            if (this.sportMeta !== null && this.sportMeta.length > 0) {
                if (this.gameReportMeta === null) {
                    this.gameReportMeta = [];
                }

                this.sportMeta.forEach(meta => {
                    if (!this.gameReportMeta.some(grm => { return (grm.sportGameMetaId === meta.sportGameMetaId); })) {
                        this.gameReportMeta.push({ sportGameMetaId: meta.sportGameMetaId, metaValue: "" });
                    }
                });

            }
        },

        methods: {
            getMetaValue(sportGameMetaId) {
                var oMeta = this.gameReportMeta.find(grm => {
                    return grm.sportGameMetaId === sportGameMetaId;
                });
                var ret = "";
                if (oMeta !== null) {
                    ret = oMeta.metaValue;
                }
                return ret;
            },

            setMetaValue(sportGameMetaId, newVal) {
                var oMeta = this.gameReportMeta.find(grm => {
                    return grm.sportGameMetaId === sportGameMetaId;
                });
                if (oMeta === null) {
                    this.gameReportMeta.push({ sportGameMetaId: sportGameMetaId, metaValue: newVal });
                } else {
                    oMeta.metaValue = newVal;
                }
                this.$emit("gamemetaupdated", this.gameReportMeta);
            }
        }
    }
</script>