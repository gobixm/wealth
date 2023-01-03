<script lang="ts">
  import Button from "@smui/button/src/Button.svelte";
  import SegmentedButton, { Label, Segment } from "@smui/segmented-button";
  let operations = ["Buy", "Sell"];
  let selectedOperation = "Buy";
</script>

<div>
  <SegmentedButton
    segments={operations}
    let:segment
    singleSelect
    bind:selected={selectedOperation}
  >
    <Segment {segment}>
      <Label>{segment}</Label>
    </Segment>
  </SegmentedButton>
</div>
