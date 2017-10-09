using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

/*
// from: https://github.com/mxgmn/WaveFunctionCollapse
public class WaveFunctionCollapse : MonoBehaviour
{
    private struct Pattern
    {
        public bool observed;
    }

    private struct WaveElement
    {
        public List<Pattern> patterns;

        //public bool unobserved;
        public int entropy;

        public WaveElement()
        {
           // unobserved = true;
        }
    }


    public Texture2D InputTexture;
    public int OutputWidth = 256;
    public int OutputHeight = 256;
    public int N = 3;

    private int OX;
    private int OY;
    private int NPatterns;

    protected bool[][] changes;
    protected double[] stationary;
    protected int[][] observed;

    void Start ()
	{
	    PerformAlgorithm();
	}

    bool OnBoundary(int x, int y)
    {
        return x + N > OX || y + N > OY;
    } 

    void CountPatterns(Color[] inPixels, int N)
    {
        // Get all NxN patterns in the data and count them (probability distribution)

        Func<int, int, int> mySumImplementation =
            delegate (int a, int b) { return a + b; };

        //Func<int, int> outF1 = i => { return i; };
        //Func<Func<int,int>, int, int> outF2 = (func, i) => { return func(i); };
        
        Func<Func<int,int,byte>, byte[]> FOnPattern = (F) =>
        {
            byte[] result = new byte[N * N];
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                    result[x + y * N] = F(x, y);
            return result;
        };


    }

    private WaveElement[,] wave;
    private Texture2D OutputTexture;

    void PerformAlgorithm ()
	{
        OX = OutputWidth;
        OY = OutputHeight;
	    
        // Count the NxN patterns (distribution)
        Color[] inPixels = InputTexture.GetPixels();
	    CountPatterns(inPixels, N);

        // Initialise output wave as unobserved everywhere
	    wave = new WaveElement[OutputHeight,OutputHeight];

	    int NPatterns = 50;
        float logT = Mathf.Log(NPatterns);
        double[] logProb = new double[NPatterns];
        for (int t = 0; t < NPatterns; t++) logProb[t] = Math.Log(stationary[t]);

        // Create patterns
        /*System.Action<byte[]> pattern = new System.Action<byte[]>(>
        {
            byte[] result = new byte[N * N];
            for (int y = 0; y < N; y++) for (int x = 0; x < N; x++) result[x + y * N] = f(x, y);
            return result;
        };*/

        // Func<int, int, byte[]> patternFromSample = (x, y) => pattern((dx, dy) => sample[(x + dx) % SMX, (y + dy) % SMY]);
        //Func<byte[], byte[]> rotate = p => pattern((x, y) => p[N - 1 - y + x * N]);
        // Func<byte[], byte[]> reflect = p => pattern((x, y) => p[N - 1 - x + y * N]);

/*

        while (true)
        {
            // Observe();

            // Propagate();

            // Find a wave element with the minimal nonzero entropy.
            var bestElement = wave.Where(x => x.entropy > 0).Min(x => x.entropy);

	        // If there is no such elements (if all elements have zero or undefined entropy) then break the cycle (4) and go to step(5).
	        if (bestElement == null)
	        {
	            break;
	        }

            // Collapse this element into a definite state according to its coefficients and the distribution of NxN patterns in the input.
        }

        
    }


    void Observe()
    {
        float min = 1000000f;
        float sum, mainSum, logSum, noise, entropy;
        int argminx = -1, argminy = -1;
        //bool[] w;

        for (int x = 0; x < OX; x++)
            for (int y = 0; y < OY; y++)
            {
                //if (OnBoundary(x, y)) continue;

                // Check what patterns are here
                var w = wave[x,y];
                var amount = 0;
                sum = 0;

                for (int t = 0; t < T; t++)
                    if (w[t])
                    {
                        amount += 1;
                        sum += stationary[t];
                    }

                if (sum == 0) return false; // no pattern here

                // Compute entropy for the present patterns
                if (amount == 1) entropy = 0;   // 1 pattern -> no entropy
                else if (amount == T) entropy = logT;   // all patterns -> max entropy
                else
                {
                    mainSum = 0;
                    logSum = Math.Log(sum);
                    for (int t = 0; t < T; t++) if (w[t]) mainSum += stationary[t] * logProb[t];
                    entropy = logSum - mainSum / sum;
                }

                noise = 0.000001f * UnityEngine.Random.value;
                if (entropy > 0 && entropy + noise < min)
                {
                    min = entropy + noise;
                    argminx = x;
                    argminy = y;
                }
            }

        if (argminx == -1 && argminy == -1)
        {
            observed = new int[FMX][];
            for (int x = 0; x < FMX; x++)
            {
                observed[x] = new int[FMY];
                for (int y = 0; y < FMY; y++) for (int t = 0; t < T; t++) if (wave[x][y][t])
                        {
                            observed[x][y] = t;
                            break;
                        }
            }

            return true;
        }

        double[] distribution = new double[T];
        for (int t = 0; t < T; t++) distribution[t] = wave[argminx][argminy][t] ? stationary[t] : 0;
        int r = distribution.Random(random.NextDouble());
        for (int t = 0; t < T; t++) wave[argminx][argminy][t] = t == r;
        changes[argminx][argminy] = true;

        return null;
    }


    bool Propagate()
    {
        // Propagate the output

        bool change = false, b;
        int x2, y2;

        for (int x1 = 0; x1 < FMX; x1++) for (int y1 = 0; y1 < FMY; y1++) if (changes[x1][y1])
                {
                    changes[x1][y1] = false;
                    for (int dx = -N + 1; dx < N; dx++) for (int dy = -N + 1; dy < N; dy++)
                        {
                            x2 = x1 + dx;
                            if (x2 < 0) x2 += FMX;
                            else if (x2 >= FMX) x2 -= FMX;

                            y2 = y1 + dy;
                            if (y2 < 0) y2 += FMY;
                            else if (y2 >= FMY) y2 -= FMY;

                            if (!periodic && (x2 + N > FMX || y2 + N > FMY)) continue;

                            bool[] w1 = wave[x1][y1];
                            bool[] w2 = wave[x2][y2];
                            int[][] p = propagator[N - 1 - dx][N - 1 - dy];

                            for (int t2 = 0; t2 < T; t2++)
                            {
                                if (!w2[t2]) continue;

                                b = false;
                                int[] prop = p[t2];
                                for (int i1 = 0; i1 < prop.Length && !b; i1++) b = w1[prop[i1]];

                                if (!b)
                                {
                                    changes[x2][y2] = true;
                                    change = true;
                                    w2[t2] = false;
                                }
                            }
                        }
                }

        return change;
    }

    void Draw()
    {
        if (!OutputTexture) OutputTexture = new Texture2D(OX, OY, TextureFormat.ARGB32, false, false);
        Color[] outColors = new Color[OX * OY];

        if (observed != null)
        {
            // Observed: only the observed patterns contributes
            for (int y = 0; y < OY; y++)
            {
                int dy = y < OY - N + 1 ? 0 : N - 1;
                for (int x = 0; x < OX; x++)
                {
                    int dx = x < OX - N + 1 ? 0 : N - 1;
                    Color c = colors[patterns[observed[x - dx][y - dy]][dx + dy * N]];
                    outColors[x + y * OX] = c;
                }
            }
        }
        else
        {
            // Not observed: each pattern should contribute
            for (int y = 0; y < OY; y++)
                for (int x = 0; x < OX; x++)
                {
                    Color finalColor = new Color(0, 0, 0, 1);
                    int nContributors = 0;
                    for (int dy = 0; dy < N; dy++)
                        for (int dx = 0; dx < N; dx++)
                        {
                            int sx = x - dx;
                            if (sx < 0) sx += OX;

                            int sy = y - dy;
                            if (sy < 0) sy += OY;

                            //if (OnBoundary(sx, sy)) continue;
                            for (int pat_i = 0; pat_i < NPatterns; pat_i++)
                                if (wave[sx,sy].observedPatterns[pat_i])
                                {
                                    nContributors++;
                                    Color addedColor = new Color();//colors[patterns[pat_i][dx + dy * N]];
                                    finalColor.r += addedColor.r;
                                    finalColor.g += addedColor.g;
                                    finalColor.b += addedColor.b;
                                }
                        }

                    outColors[x + y * OX] = finalColor / nContributors;
                }
        }

        OutputTexture.SetPixels(outColors);
    }
}
*/