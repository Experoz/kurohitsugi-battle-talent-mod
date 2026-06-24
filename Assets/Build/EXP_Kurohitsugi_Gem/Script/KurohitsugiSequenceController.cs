using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KurohitsugiSequenceController : MonoBehaviour
{
    [Header("Main Roots")]
    public GameObject linesRoot;
    public GameObject wallsRoot;
    public GameObject topRoot;
    public GameObject spikesRoot;
    public GameObject effectsRoot;
    public GameObject breakPiecesRoot;

    [Header("Sequence Timings")]
    public float delayBeforeStart = 0.1f;
    public float linesDelay = 0.15f;
    public float wallsDelay = 0.35f;
    public float topDelay = 0.25f;
    public float effectsDelay = 0.25f;
    public float spikesDelay = 0.35f;

    [Header("Line Animation")]
    public float verticalLinesGrowDuration = 0.25f;
    public float topLinesGrowDuration = 0.18f;

    [Header("Wall Animation")]
    public float wallsGrowDuration = 0.5f;
    public float topGrowDuration = 0.3f;

    [Header("Spike Animation")]
    public float spikeAppearStepDelay = 0.08f;
    public float spikeGrowDuration = 0.12f;
    public float spikeOvershootMultiplier = 1.18f;

    [Header("End Sequence")]
    public float holdAfterSpikes = 1.0f;
    public float spikeRetractDuration = 0.25f;
    public bool hideAfterRetract = true;

    [Header("Break Pieces Animation")]
    public bool playBreakPieces = true;
    public float breakPiecesDuration = 0.55f;
    public float breakPiecesHoldDuration = 0.45f;
    public float breakPiecesOutwardDistance = 0.45f;
    public float breakPiecesStartScaleMultiplier = 0.65f;
    public float breakPiecesEndScaleMultiplier = 1.0f;

    [Header("Auto Start")]
    public bool playOnStart = true;

    private Coroutine sequenceCoroutine;

    private readonly List<Transform> lineList = new List<Transform>();
    private readonly List<Vector3> lineOriginalScales = new List<Vector3>();
    private readonly List<Vector3> lineOriginalPositions = new List<Vector3>();

    private readonly List<Transform> wallList = new List<Transform>();
    private readonly List<Vector3> wallOriginalScales = new List<Vector3>();
    private readonly List<Vector3> wallOriginalPositions = new List<Vector3>();

    private readonly List<Transform> topList = new List<Transform>();
    private readonly List<Vector3> topOriginalScales = new List<Vector3>();

    private readonly List<Transform> spikeList = new List<Transform>();
    private readonly List<Vector3> spikeOriginalScales = new List<Vector3>();

    private readonly List<Transform> breakPieceList = new List<Transform>();
    private readonly List<Vector3> breakPieceOriginalPositions = new List<Vector3>();
    private readonly List<Vector3> breakPieceOriginalScales = new List<Vector3>();
    private readonly List<Quaternion> breakPieceOriginalRotations = new List<Quaternion>();

    private bool cached = false;

    private void Awake()
    {
        CacheOriginalTransforms();
    }

    private void Start()
    {
        ResetSequence();

        if (playOnStart)
        {
            sequenceCoroutine = StartCoroutine(PlaySequence());
        }
    }

    private void CacheOriginalTransforms()
    {
        CacheLines();
        CacheWalls();
        CacheTop();
        CacheSpikes();
        CacheBreakPieces();

        cached = true;
    }

    private void CacheLines()
    {
        lineList.Clear();
        lineOriginalScales.Clear();
        lineOriginalPositions.Clear();

        if (linesRoot == null)
        {
            Debug.LogWarning("KurohitsugiSequenceController: LinesRoot non assegnato.");
            return;
        }

        foreach (Transform line in linesRoot.transform)
        {
            if (line == null)
            {
                continue;
            }

            lineList.Add(line);
            lineOriginalScales.Add(line.localScale);
            lineOriginalPositions.Add(line.localPosition);
        }

        Debug.Log("KurohitsugiSequenceController: linee trovate = " + lineList.Count);
    }

    private void CacheWalls()
    {
        wallList.Clear();
        wallOriginalScales.Clear();
        wallOriginalPositions.Clear();

        if (wallsRoot == null)
        {
            Debug.LogWarning("KurohitsugiSequenceController: WallsRoot non assegnato.");
            return;
        }

        foreach (Transform wall in wallsRoot.transform)
        {
            if (wall == null)
            {
                continue;
            }

            wallList.Add(wall);
            wallOriginalScales.Add(wall.localScale);
            wallOriginalPositions.Add(wall.localPosition);
        }

        Debug.Log("KurohitsugiSequenceController: pareti trovate = " + wallList.Count);
    }

    private void CacheTop()
    {
        topList.Clear();
        topOriginalScales.Clear();

        if (topRoot == null)
        {
            Debug.LogWarning("KurohitsugiSequenceController: TopRoot non assegnato.");
            return;
        }

        foreach (Transform top in topRoot.transform)
        {
            if (top == null)
            {
                continue;
            }

            topList.Add(top);
            topOriginalScales.Add(top.localScale);
        }

        Debug.Log("KurohitsugiSequenceController: top trovati = " + topList.Count);
    }

    private void CacheSpikes()
    {
        spikeList.Clear();
        spikeOriginalScales.Clear();

        if (spikesRoot == null)
        {
            Debug.LogWarning("KurohitsugiSequenceController: SpikesRoot non assegnato.");
            return;
        }

        foreach (Transform spike in spikesRoot.transform)
        {
            if (spike == null)
            {
                continue;
            }

            spikeList.Add(spike);
            spikeOriginalScales.Add(spike.localScale);
        }

        Debug.Log("KurohitsugiSequenceController: spike trovati = " + spikeList.Count);
    }

    private void CacheBreakPieces()
    {
        breakPieceList.Clear();
        breakPieceOriginalPositions.Clear();
        breakPieceOriginalScales.Clear();
        breakPieceOriginalRotations.Clear();

        if (breakPiecesRoot == null)
        {
            Debug.LogWarning("KurohitsugiSequenceController: BreakPiecesRoot non assegnato.");
            return;
        }

        foreach (Transform piece in breakPiecesRoot.transform)
        {
            if (piece == null)
            {
                continue;
            }

            breakPieceList.Add(piece);
            breakPieceOriginalPositions.Add(piece.localPosition);
            breakPieceOriginalScales.Add(piece.localScale);
            breakPieceOriginalRotations.Add(piece.localRotation);
        }

        Debug.Log("KurohitsugiSequenceController: frammenti trovati = " + breakPieceList.Count);
    }

    public void StartKurohitsugiSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        if (!cached)
        {
            CacheOriginalTransforms();
        }

        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    public void ResetSequence()
    {
        SetRootActive(effectsRoot, false);

        ResetLines();
        ResetWalls();
        ResetTop();
        ResetSpikes();
        ResetBreakPieces();
    }

    private void ResetLines()
    {
        if (linesRoot != null)
        {
            linesRoot.SetActive(true);
        }

        for (int i = 0; i < lineList.Count; i++)
        {
            Transform line = lineList[i];

            if (line == null)
            {
                continue;
            }

            Vector3 finalScale = lineOriginalScales[i];
            Vector3 finalPosition = lineOriginalPositions[i];

            line.gameObject.SetActive(false);

            if (IsVerticalCornerLine(line))
            {
                line.localScale = new Vector3(
                    finalScale.x,
                    0.01f,
                    finalScale.z
                );

                line.localPosition = new Vector3(
                    finalPosition.x,
                    0.005f,
                    finalPosition.z
                );
            }
            else
            {
                line.localScale = GetHiddenTopLineScale(finalScale);
                line.localPosition = finalPosition;
            }
        }
    }

    private void ResetWalls()
    {
        if (wallsRoot != null)
        {
            wallsRoot.SetActive(true);
        }

        for (int i = 0; i < wallList.Count; i++)
        {
            Transform wall = wallList[i];

            if (wall == null)
            {
                continue;
            }

            Vector3 finalScale = wallOriginalScales[i];
            Vector3 finalPosition = wallOriginalPositions[i];

            wall.gameObject.SetActive(false);

            wall.localScale = new Vector3(
                finalScale.x,
                0.01f,
                finalScale.z
            );

            wall.localPosition = new Vector3(
                finalPosition.x,
                0.005f,
                finalPosition.z
            );
        }
    }

    private void ResetTop()
    {
        if (topRoot != null)
        {
            topRoot.SetActive(true);
        }

        for (int i = 0; i < topList.Count; i++)
        {
            Transform top = topList[i];

            if (top == null)
            {
                continue;
            }

            Vector3 finalScale = topOriginalScales[i];

            top.gameObject.SetActive(false);

            top.localScale = new Vector3(
                finalScale.x * 0.05f,
                finalScale.y,
                finalScale.z * 0.05f
            );
        }
    }

    private void ResetSpikes()
    {
        if (spikesRoot != null)
        {
            spikesRoot.SetActive(true);
        }

        for (int i = 0; i < spikeList.Count; i++)
        {
            Transform spike = spikeList[i];

            if (spike == null)
            {
                continue;
            }

            Vector3 targetScale = spikeOriginalScales[i];

            spike.gameObject.SetActive(false);
            spike.localScale = GetHiddenSpikeScale(targetScale);
        }
    }

    private void ResetBreakPieces()
    {
        if (breakPiecesRoot != null)
        {
            breakPiecesRoot.SetActive(false);
        }

        for (int i = 0; i < breakPieceList.Count; i++)
        {
            Transform piece = breakPieceList[i];

            if (piece == null)
            {
                continue;
            }

            piece.gameObject.SetActive(false);
            piece.localPosition = breakPieceOriginalPositions[i];
            piece.localScale = breakPieceOriginalScales[i] * breakPiecesStartScaleMultiplier;
            piece.localRotation = breakPieceOriginalRotations[i];
        }
    }

    private IEnumerator PlaySequence()
    {
        ResetSequence();

        yield return new WaitForSeconds(delayBeforeStart);

        yield return StartCoroutine(GrowLines());

        yield return new WaitForSeconds(linesDelay);

        yield return StartCoroutine(GrowWalls());

        yield return new WaitForSeconds(wallsDelay);

        yield return StartCoroutine(GrowTop());

        yield return new WaitForSeconds(topDelay);

        SetRootActive(effectsRoot, true);

        yield return new WaitForSeconds(effectsDelay);

        yield return StartCoroutine(ShowSpikesAllTogether());

        yield return new WaitForSeconds(holdAfterSpikes);

        yield return StartCoroutine(RetractSpikesAllTogether());

        if (playBreakPieces)
        {
            HideMainStructureForBreak();
            yield return StartCoroutine(PlayBreakPiecesAnimation());
        }

        if (hideAfterRetract)
        {
            HideEverything();
        }

        yield return new WaitForSeconds(spikesDelay);
    }

    private IEnumerator GrowLines()
    {
        if (linesRoot == null)
        {
            yield break;
        }

        linesRoot.SetActive(true);

        for (int i = 0; i < lineList.Count; i++)
        {
            Transform line = lineList[i];

            if (line == null)
            {
                continue;
            }

            line.gameObject.SetActive(true);
        }

        yield return StartCoroutine(GrowVerticalCornerLines());
        yield return StartCoroutine(GrowTopEdgeLines());
    }

    private IEnumerator GrowVerticalCornerLines()
    {
        float timer = 0f;

        while (timer < verticalLinesGrowDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / verticalLinesGrowDuration);
            float eased = EaseOutCubic(t);

            for (int i = 0; i < lineList.Count; i++)
            {
                Transform line = lineList[i];

                if (line == null || !IsVerticalCornerLine(line))
                {
                    continue;
                }

                Vector3 finalScale = lineOriginalScales[i];
                Vector3 finalPosition = lineOriginalPositions[i];

                float currentHeight = Mathf.Lerp(0.01f, finalScale.y, eased);

                line.localScale = new Vector3(
                    finalScale.x,
                    currentHeight,
                    finalScale.z
                );

                line.localPosition = new Vector3(
                    finalPosition.x,
                    currentHeight * 0.5f,
                    finalPosition.z
                );
            }

            yield return null;
        }

        for (int i = 0; i < lineList.Count; i++)
        {
            Transform line = lineList[i];

            if (line == null || !IsVerticalCornerLine(line))
            {
                continue;
            }

            line.localScale = lineOriginalScales[i];
            line.localPosition = lineOriginalPositions[i];
        }
    }

    private IEnumerator GrowTopEdgeLines()
    {
        float timer = 0f;

        while (timer < topLinesGrowDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / topLinesGrowDuration);
            float eased = EaseOutCubic(t);

            for (int i = 0; i < lineList.Count; i++)
            {
                Transform line = lineList[i];

                if (line == null || IsVerticalCornerLine(line))
                {
                    continue;
                }

                Vector3 finalScale = lineOriginalScales[i];
                Vector3 startScale = GetHiddenTopLineScale(finalScale);

                line.localScale = Vector3.Lerp(startScale, finalScale, eased);
                line.localPosition = lineOriginalPositions[i];
            }

            yield return null;
        }

        for (int i = 0; i < lineList.Count; i++)
        {
            Transform line = lineList[i];

            if (line == null || IsVerticalCornerLine(line))
            {
                continue;
            }

            line.localScale = lineOriginalScales[i];
            line.localPosition = lineOriginalPositions[i];
        }
    }

    private IEnumerator GrowWalls()
    {
        if (wallsRoot == null)
        {
            yield break;
        }

        wallsRoot.SetActive(true);

        for (int i = 0; i < wallList.Count; i++)
        {
            Transform wall = wallList[i];

            if (wall == null)
            {
                continue;
            }

            wall.gameObject.SetActive(true);
        }

        float timer = 0f;

        while (timer < wallsGrowDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / wallsGrowDuration);
            float eased = EaseOutCubic(t);

            for (int i = 0; i < wallList.Count; i++)
            {
                Transform wall = wallList[i];

                if (wall == null)
                {
                    continue;
                }

                Vector3 finalScale = wallOriginalScales[i];
                Vector3 finalPosition = wallOriginalPositions[i];

                float currentHeight = Mathf.Lerp(0.01f, finalScale.y, eased);

                wall.localScale = new Vector3(
                    finalScale.x,
                    currentHeight,
                    finalScale.z
                );

                wall.localPosition = new Vector3(
                    finalPosition.x,
                    currentHeight * 0.5f,
                    finalPosition.z
                );
            }

            yield return null;
        }

        for (int i = 0; i < wallList.Count; i++)
        {
            Transform wall = wallList[i];

            if (wall == null)
            {
                continue;
            }

            wall.localScale = wallOriginalScales[i];
            wall.localPosition = wallOriginalPositions[i];
        }
    }

    private IEnumerator GrowTop()
    {
        if (topRoot == null)
        {
            yield break;
        }

        topRoot.SetActive(true);

        for (int i = 0; i < topList.Count; i++)
        {
            Transform top = topList[i];

            if (top == null)
            {
                continue;
            }

            top.gameObject.SetActive(true);
        }

        float timer = 0f;

        while (timer < topGrowDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / topGrowDuration);
            float eased = EaseOutCubic(t);

            for (int i = 0; i < topList.Count; i++)
            {
                Transform top = topList[i];

                if (top == null)
                {
                    continue;
                }

                Vector3 finalScale = topOriginalScales[i];

                top.localScale = new Vector3(
                    Mathf.Lerp(finalScale.x * 0.05f, finalScale.x, eased),
                    finalScale.y,
                    Mathf.Lerp(finalScale.z * 0.05f, finalScale.z, eased)
                );
            }

            yield return null;
        }

        for (int i = 0; i < topList.Count; i++)
        {
            Transform top = topList[i];

            if (top == null)
            {
                continue;
            }

            top.localScale = topOriginalScales[i];
        }
    }

    private IEnumerator ShowSpikesAllTogether()
    {
        if (spikesRoot == null)
        {
            yield break;
        }

        spikesRoot.SetActive(true);

        for (int i = 0; i < spikeList.Count; i++)
        {
            Transform spike = spikeList[i];

            if (spike == null)
            {
                continue;
            }

            Vector3 targetScale = spikeOriginalScales[i];

            spike.gameObject.SetActive(true);
            spike.localScale = GetHiddenSpikeScale(targetScale);

            StartCoroutine(AnimateSpikeGrow(spike, targetScale));
        }

        yield return new WaitForSeconds(spikeGrowDuration);
    }

    private IEnumerator AnimateSpikeGrow(Transform spike, Vector3 targetScale)
    {
        if (spike == null)
        {
            yield break;
        }

        Vector3 startScale = GetHiddenSpikeScale(targetScale);
        Vector3 overshootScale = targetScale * spikeOvershootMultiplier;

        float firstPhase = spikeGrowDuration * 0.7f;
        float secondPhase = spikeGrowDuration * 0.3f;

        float timer = 0f;

        while (timer < firstPhase)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / firstPhase);
            float eased = EaseOutCubic(t);

            spike.localScale = Vector3.Lerp(startScale, overshootScale, eased);

            yield return null;
        }

        timer = 0f;

        while (timer < secondPhase)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / secondPhase);
            float eased = EaseOutCubic(t);

            spike.localScale = Vector3.Lerp(overshootScale, targetScale, eased);

            yield return null;
        }

        spike.localScale = targetScale;
        spike.gameObject.SetActive(true);
    }

    private IEnumerator RetractSpikesAllTogether()
    {
        if (spikesRoot == null)
        {
            yield break;
        }

        spikesRoot.SetActive(true);

        float timer = 0f;

        List<Vector3> startScales = new List<Vector3>();

        for (int i = 0; i < spikeList.Count; i++)
        {
            Transform spike = spikeList[i];

            if (spike == null)
            {
                startScales.Add(Vector3.zero);
                continue;
            }

            spike.gameObject.SetActive(true);
            startScales.Add(spike.localScale);
        }

        while (timer < spikeRetractDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / spikeRetractDuration);
            float eased = EaseInCubic(t);

            for (int i = 0; i < spikeList.Count; i++)
            {
                Transform spike = spikeList[i];

                if (spike == null)
                {
                    continue;
                }

                Vector3 hiddenScale = GetHiddenSpikeScale(spikeOriginalScales[i]);

                spike.localScale = Vector3.Lerp(
                    startScales[i],
                    hiddenScale,
                    eased
                );
            }

            yield return null;
        }

        for (int i = 0; i < spikeList.Count; i++)
        {
            Transform spike = spikeList[i];

            if (spike == null)
            {
                continue;
            }

            spike.localScale = GetHiddenSpikeScale(spikeOriginalScales[i]);
            spike.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayBreakPiecesAnimation()
    {
        if (breakPiecesRoot == null)
        {
            yield break;
        }

        breakPiecesRoot.SetActive(true);

        for (int i = 0; i < breakPieceList.Count; i++)
        {
            Transform piece = breakPieceList[i];

            if (piece == null)
            {
                continue;
            }

            piece.gameObject.SetActive(true);

            piece.localPosition = breakPieceOriginalPositions[i];
            piece.localScale = breakPieceOriginalScales[i] * breakPiecesStartScaleMultiplier;
            piece.localRotation = breakPieceOriginalRotations[i];
        }

        float timer = 0f;

        while (timer < breakPiecesDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / breakPiecesDuration);
            float eased = EaseOutCubic(t);

            for (int i = 0; i < breakPieceList.Count; i++)
            {
                Transform piece = breakPieceList[i];

                if (piece == null)
                {
                    continue;
                }

                Vector3 startPos = breakPieceOriginalPositions[i];
                Vector3 outwardDir = GetOutwardDirection(startPos);
                Vector3 endPos = startPos + outwardDir * breakPiecesOutwardDistance;

                Vector3 startScale = breakPieceOriginalScales[i] * breakPiecesStartScaleMultiplier;
                Vector3 endScale = breakPieceOriginalScales[i] * breakPiecesEndScaleMultiplier;

                piece.localPosition = Vector3.Lerp(startPos, endPos, eased);
                piece.localScale = Vector3.Lerp(startScale, endScale, eased);

                piece.Rotate(
                    outwardDir.x * 40f * Time.deltaTime,
                    35f * Time.deltaTime,
                    outwardDir.z * 40f * Time.deltaTime,
                    Space.Self
                );
            }

            yield return null;
        }

        yield return new WaitForSeconds(breakPiecesHoldDuration);
    }

    private Vector3 GetOutwardDirection(Vector3 localPosition)
    {
        Vector3 direction = new Vector3(localPosition.x, localPosition.y - 3f, localPosition.z);

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector3.up;
        }

        return direction.normalized;
    }

    private bool IsVerticalCornerLine(Transform line)
    {
        if (line == null)
        {
            return false;
        }

        return line.name.ToLower().Contains("corner");
    }

    private Vector3 GetHiddenTopLineScale(Vector3 finalScale)
    {
        Vector3 hiddenScale = finalScale;

        if (finalScale.x >= finalScale.y && finalScale.x >= finalScale.z)
        {
            hiddenScale.x = 0.01f;
        }
        else if (finalScale.z >= finalScale.x && finalScale.z >= finalScale.y)
        {
            hiddenScale.z = 0.01f;
        }
        else
        {
            hiddenScale.y = 0.01f;
        }

        return hiddenScale;
    }

    private void HideMainStructureForBreak()
    {
        SetRootActive(linesRoot, false);
        SetRootActive(wallsRoot, false);
        SetRootActive(topRoot, false);

        if (spikesRoot != null)
        {
            for (int i = 0; i < spikeList.Count; i++)
            {
                Transform spike = spikeList[i];

                if (spike == null)
                {
                    continue;
                }

                spike.gameObject.SetActive(false);
            }
        }

        // EffectsRoot resta acceso durante la frantumazione.
    }

    private void HideEverything()
    {
        SetRootActive(linesRoot, false);
        SetRootActive(wallsRoot, false);
        SetRootActive(topRoot, false);
        SetRootActive(effectsRoot, false);
        SetRootActive(breakPiecesRoot, false);

        if (spikesRoot != null)
        {
            spikesRoot.SetActive(true);

            for (int i = 0; i < spikeList.Count; i++)
            {
                Transform spike = spikeList[i];

                if (spike == null)
                {
                    continue;
                }

                spike.gameObject.SetActive(false);
            }
        }
    }

    private Vector3 GetHiddenSpikeScale(Vector3 targetScale)
    {
        return new Vector3(
            Mathf.Max(targetScale.x * 0.2f, 0.01f),
            Mathf.Max(targetScale.y * 0.03f, 0.01f),
            Mathf.Max(targetScale.z * 0.2f, 0.01f)
        );
    }

    private float EaseOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private float EaseInCubic(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * t;
    }

    private void SetRootActive(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }
}