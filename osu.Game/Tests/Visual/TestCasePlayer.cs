﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.IO;
using System.Linq;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play;
using osu.Game.Tests.Beatmaps;
using OpenTK.Graphics;

namespace osu.Game.Tests.Visual
{
    public abstract class TestCasePlayer : ScreenTestCase
    {
        private readonly Type ruleset;

        protected Player Player;

        /// <summary>
        /// Create a TestCase which runs through the Player screen.
        /// </summary>
        /// <param name="ruleset">An optional ruleset type which we want to target. If not provided we'll allow all rulesets to be tested.</param>
        protected TestCasePlayer(Type ruleset)
        {
            this.ruleset = ruleset;
        }

        protected TestCasePlayer()
        {

        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
            Add(new Box
            {
                RelativeSizeAxes = Framework.Graphics.Axes.Both,
                Colour = Color4.Black,
                Depth = int.MaxValue
            });

            string instantiation = ruleset?.AssemblyQualifiedName;

            foreach (var r in rulesets.AvailableRulesets.Where(rs => instantiation == null || rs.InstantiationInfo == instantiation))
            {
                Player p = null;
                AddStep(r.Name, () => p = loadPlayerFor(r));
                AddUntilStep(() => p.IsLoaded);
            }
        }

        protected virtual Beatmap CreateBeatmap()
        {
            Beatmap beatmap;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(test_beatmap_data)))
            using (var reader = new StreamReader(stream))
                beatmap = BeatmapDecoder.GetDecoder(reader).DecodeBeatmap(reader);

            return beatmap;
        }

        private Player loadPlayerFor(RulesetInfo r)
        {
            var beatmap = CreateBeatmap();

            beatmap.BeatmapInfo.Ruleset = r;

            var instance = r.CreateInstance();

            WorkingBeatmap working = new TestWorkingBeatmap(beatmap);
            working.Mods.Value = new[] { instance.GetAllMods().First(m => m is ModNoFail) };

            if (Player != null)
                Remove(Player);

            var player = CreatePlayer(working, instance);

            LoadComponentAsync(player, LoadScreen);

            return player;
        }

        protected virtual Player CreatePlayer(WorkingBeatmap beatmap, Ruleset ruleset) => new Player
        {
            InitialBeatmap = beatmap,
            AllowPause = false
        };

        private const string test_beatmap_data =
            @"osu file format v14

[General]
AudioLeadIn: 500
PreviewTime: 53498
Countdown: 0
SampleSet: Soft
StackLeniency: 0.7
Mode: 0
LetterboxInBreaks: 0
WidescreenStoryboard: 1

[Editor]
DistanceSpacing: 1.2
BeatDivisor: 4
GridSize: 4
TimelineZoom: 1

[Metadata]
Title:My Love
TitleUnicode:My Love
Artist:Kuba Oms
ArtistUnicode:Kuba Oms
Creator:W h i t e
Version:Hard
Source:ADHD
Tags:Monthly Beatmapping Contest Electronic folk pop w_h_i_t_e
BeatmapID:397534
BeatmapSetID:163112

[Difficulty]
HPDrainRate:5
CircleSize:4
OverallDifficulty:6
ApproachRate:7
SliderMultiplier:1.44
SliderTickRate:2

[Events]
//Break Periods
2,69870,83770
2,152170,158770
//Storyboard Layer 0 (Background)
//Storyboard Layer 1 (Fail)
//Storyboard Layer 2 (Pass)
//Storyboard Layer 3 (Foreground)
//Storyboard Sound Samples

[TimingPoints]
2170,468.75,4,2,0,40,1,0
4045,-100,4,2,0,30,0,0
4162,-100,4,2,0,40,0,0
5920,-100,4,2,0,30,0,0
6037,-100,4,2,0,40,0,0
7795,-100,4,2,0,30,0,0
7912,-100,4,2,0,40,0,0
9670,-100,4,2,0,40,0,0
9787,-100,4,2,0,50,0,0
11545,-100,4,2,0,40,0,0
11662,-100,4,2,0,50,0,0
13420,-100,4,2,0,40,0,0
13537,-100,4,2,0,50,0,0
15295,-100,4,2,0,40,0,0
15412,-100,4,2,0,50,0,0
17170,-100,4,2,0,40,0,0
17287,-100,4,2,0,50,0,0
19045,-100,4,2,0,40,0,0
19162,-100,4,2,0,50,0,0
20920,-100,4,2,0,40,0,0
21037,-100,4,2,0,50,0,0
22795,-100,4,2,0,40,0,0
22912,-100,4,2,0,50,0,0
24670,-100,4,2,0,70,0,0
37560,-200,4,2,0,30,0,0
38263,-200,4,2,0,5,0,0
38966,-100,4,2,0,30,0,0
39670,-100,4,2,0,70,0,0
53732,-100,4,2,0,40,0,0
54670,-100,4,2,0,80,0,1
55138,-100,4,2,0,60,0,1
55255,-100,4,2,0,80,0,1
56076,-100,4,2,0,60,0,1
56193,-100,4,2,0,80,0,1
57013,-100,4,2,0,60,0,1
57130,-100,4,2,0,80,0,1
57951,-100,4,2,0,60,0,1
58068,-100,4,2,0,80,0,1
58888,-100,4,2,0,60,0,1
59005,-100,4,2,0,80,0,1
59826,-100,4,2,0,60,0,1
59943,-100,4,2,0,80,0,1
60763,-100,4,2,0,60,0,1
60880,-100,4,2,0,80,0,1
61701,-100,4,2,0,60,0,1
61818,-100,4,2,0,80,0,1
62638,-100,4,2,0,60,0,1
62755,-100,4,2,0,80,0,1
63576,-100,4,2,0,60,0,1
63693,-100,4,2,0,80,0,1
64513,-100,4,2,0,60,0,1
64630,-100,4,2,0,80,0,1
65451,-100,4,2,0,60,0,1
65568,-100,4,2,0,80,0,1
66388,-100,4,2,0,60,0,1
66505,-100,4,2,0,80,0,1
67326,-100,4,2,0,60,0,1
67443,-100,4,2,0,80,0,1
68263,-100,4,2,0,60,0,1
68380,-100,4,2,0,80,0,1
69201,-100,4,2,0,60,0,1
69318,-100,4,2,0,80,0,1
69670,-100,4,2,0,70,0,0
84670,-100,4,2,0,70,0,0
97560,-200,4,2,0,70,0,0
97795,-200,4,2,0,30,0,0
98966,-100,4,2,0,30,0,0
99670,-100,4,2,0,70,0,0
113732,-100,4,2,0,40,0,0
114670,-100,4,2,0,80,0,1
115138,-100,4,2,0,60,0,1
115255,-100,4,2,0,80,0,1
116076,-100,4,2,0,60,0,1
116193,-100,4,2,0,80,0,1
117013,-100,4,2,0,60,0,1
117130,-100,4,2,0,80,0,1
117951,-100,4,2,0,60,0,1
118068,-100,4,2,0,80,0,1
118888,-100,4,2,0,60,0,1
119005,-100,4,2,0,80,0,1
119826,-100,4,2,0,60,0,1
119943,-100,4,2,0,80,0,1
120763,-100,4,2,0,60,0,1
120880,-100,4,2,0,80,0,1
121701,-100,4,2,0,60,0,1
121818,-100,4,2,0,80,0,1
122638,-100,4,2,0,60,0,1
122755,-100,4,2,0,80,0,1
123576,-100,4,2,0,60,0,1
123693,-100,4,2,0,80,0,1
124513,-100,4,2,0,60,0,1
124630,-100,4,2,0,80,0,1
125451,-100,4,2,0,60,0,1
125568,-100,4,2,0,80,0,1
126388,-100,4,2,0,60,0,1
126505,-100,4,2,0,80,0,1
127326,-100,4,2,0,60,0,1
127443,-100,4,2,0,80,0,1
128263,-100,4,2,0,60,0,1
128380,-100,4,2,0,80,0,1
129201,-100,4,2,0,60,0,1
129318,-100,4,2,0,80,0,1
129670,-200,4,2,0,40,0,0
144670,-133.333333333333,4,2,0,40,0,0
159670,-133.333333333333,4,2,0,40,0,0
163420,-133.333333333333,4,2,0,45,0,0
163888,-125,4,2,0,50,0,0
164357,-117.647058823529,4,2,0,55,0,0
164826,-111.111111111111,4,2,0,60,0,0
165295,-105.263157894737,4,2,0,65,0,0
165763,-100,4,2,0,70,0,0
166232,-100,4,2,0,40,0,0
167170,-100,4,2,0,80,0,1
167638,-100,4,2,0,60,0,1
167755,-100,4,2,0,80,0,1
168576,-100,4,2,0,60,0,1
168693,-100,4,2,0,80,0,1
169513,-100,4,2,0,60,0,1
169630,-100,4,2,0,80,0,1
170451,-100,4,2,0,60,0,1
170568,-100,4,2,0,80,0,1
171388,-100,4,2,0,60,0,1
171505,-100,4,2,0,80,0,1
172326,-100,4,2,0,60,0,1
172443,-100,4,2,0,80,0,1
173263,-100,4,2,0,60,0,1
173380,-100,4,2,0,80,0,1
174201,-100,4,2,0,60,0,1
174318,-100,4,2,0,80,0,1
175138,-100,4,2,0,60,0,1
175255,-100,4,2,0,80,0,1
176076,-100,4,2,0,60,0,1
176193,-100,4,2,0,80,0,1
177013,-100,4,2,0,60,0,1
177130,-100,4,2,0,80,0,1
177951,-100,4,2,0,60,0,1
178068,-100,4,2,0,80,0,1
178888,-100,4,2,0,60,0,1
179005,-100,4,2,0,80,0,1
179826,-100,4,2,0,60,0,1
179943,-100,4,2,0,80,0,1
180763,-100,4,2,0,60,0,1
180880,-100,4,2,0,80,0,1
180998,-100,4,2,0,80,0,0
181466,-100,4,2,0,60,0,0
181584,-100,4,2,0,80,0,0
181935,-100,4,2,0,80,0,0
182170,-100,4,2,0,80,0,1
182638,-100,4,2,0,60,0,1
182755,-100,4,2,0,80,0,1
183576,-100,4,2,0,60,0,1
183693,-100,4,2,0,80,0,1
184513,-100,4,2,0,60,0,1
184630,-100,4,2,0,80,0,1
185451,-100,4,2,0,60,0,1
185568,-100,4,2,0,80,0,1
186388,-100,4,2,0,60,0,1
186505,-100,4,2,0,80,0,1
187326,-100,4,2,0,60,0,1
187443,-100,4,2,0,80,0,1
188263,-100,4,2,0,60,0,1
188380,-100,4,2,0,80,0,1
189201,-100,4,2,0,60,0,1
189318,-100,4,2,0,80,0,1
190138,-100,4,2,0,60,0,1
190255,-100,4,2,0,80,0,1
191076,-100,4,2,0,60,0,1
191193,-100,4,2,0,80,0,1
192013,-100,4,2,0,60,0,1
192130,-100,4,2,0,80,0,1
192951,-100,4,2,0,60,0,1
193068,-100,4,2,0,80,0,1
193888,-100,4,2,0,60,0,1
194005,-100,4,2,0,80,0,1
194826,-100,4,2,0,60,0,1
194943,-100,4,2,0,80,0,1
195295,-100,4,2,0,50,0,1
195529,-100,4,2,0,52,0,1
195646,-100,4,2,0,54,0,1
195763,-100,4,2,0,56,0,1
195880,-100,4,2,0,58,0,1
195998,-100,4,2,0,60,0,1
196115,-100,4,2,0,62,0,1
196232,-100,4,2,0,64,0,1
196349,-100,4,2,0,68,0,1
196466,-100,4,2,0,70,0,1
196584,-100,4,2,0,72,0,1
196701,-100,4,2,0,74,0,1
196818,-100,4,2,0,76,0,1
196935,-100,4,2,0,78,0,1
197052,-100,4,2,0,80,0,1
197170,-100,4,2,0,80,0,0
197873,-100,4,2,0,60,0,0
197990,-100,4,2,0,80,0,0
198341,-100,4,2,0,60,0,0
199045,-100,4,2,0,80,0,0
199279,-100,4,2,0,60,0,0
199630,-100,4,2,0,80,0,0
200216,-100,4,2,0,60,0,0
200334,-100,4,2,0,80,0,0
201623,-100,4,2,0,60,0,0
201740,-100,4,2,0,80,0,0
202326,-100,4,2,0,60,0,0
202443,-100,4,2,0,80,0,0
203029,-100,4,2,0,60,0,0
203498,-100,4,2,0,80,0,0
203966,-100,4,2,0,60,0,0
204201,-100,4,2,0,80,0,0
205373,-100,4,2,0,60,0,0
205490,-100,4,2,0,80,0,0
205841,-100,4,2,0,60,0,0
206076,-100,4,2,0,60,0,0
206545,-100,4,2,0,80,0,0
206779,-100,4,2,0,60,0,0
207130,-100,4,2,0,80,0,0
207716,-100,4,2,0,60,0,0
207951,-100,4,2,0,80,0,0
209123,-100,4,2,0,60,0,0
209240,-100,4,2,0,80,0,0
209826,-100,4,2,0,60,0,0
209943,-100,4,2,0,80,0,0
210529,-100,4,2,0,60,0,0
210880,-100,4,2,0,80,0,0
211232,-100,4,2,0,60,0,0
211701,-100,4,2,0,70,0,0
212170,-100,4,2,0,80,0,0
212873,-100,4,2,0,60,0,0
212990,-100,4,2,0,80,0,0
213341,-100,4,2,0,60,0,0
213576,-100,4,2,0,60,0,0
214045,-100,4,2,0,80,0,0
214279,-100,4,2,0,60,0,0
214630,-100,4,2,0,80,0,0
215216,-100,4,2,0,60,0,0
215451,-100,4,2,0,80,0,0
216623,-100,4,2,0,60,0,0
216740,-100,4,2,0,80,0,0
217326,-100,4,2,0,60,0,0
217443,-100,4,2,0,80,0,0
218029,-100,4,2,0,60,0,0
218498,-100,4,2,0,80,0,0
218732,-100,4,2,0,50,0,0
219670,-100,4,2,0,70,0,0
220138,-100,4,2,0,65,0,0
220373,-100,4,2,0,45,0,0
220490,-100,4,2,0,65,0,0
220607,-100,4,2,0,60,0,0
220841,-100,4,2,0,35,0,0
221076,-100,4,2,0,35,0,0
221545,-100,4,2,0,50,0,0
221779,-100,4,2,0,30,0,0
222013,-111.111111111111,4,2,0,25,0,0
222130,-111.111111111111,4,2,0,40,0,0
222482,-125,4,2,0,40,0,0
222716,-125,4,2,0,20,0,0
222951,-100,4,2,0,15,0,0
223420,-100,4,2,0,30,0,0
224357,-100,4,2,0,25,0,0
225295,-100,4,2,0,20,0,0
226232,-100,4,2,0,15,0,0
226701,-100,4,2,0,10,0,0
227170,-100,4,2,0,5,0,0


[Colours]
        Combo1 : 17,254,176
Combo2 : 173,255,95
Combo3 : 255,88,100
Combo4 : 255,94,55

[HitObjects]
320,256,2170,6,0,P|256:284|192:256,1,144,4|0,0:0|0:0,0:0:0:0:
144,184,2873,1,0,0:0:0:0:
108,260,3107,2,0,P|112:296|100:336,1,72
28,288,3576,2,0,P|24:252|36:212,1,72,0|0,0:0|0:0,0:0:0:0:
76,140,4045,6,0,L|220:136,1,144,4|0,0:0|0:0,0:0:0:0:
292,88,4748,1,0,0:0:0:0:
292,88,4982,2,0,P|304:120|300:168,1,72
388,168,5451,2,0,P|396:133|416:103,1,72,0|0,0:0|0:0,0:0:0:0:
472,172,5920,6,0,B|470:200|457:222|457:222|488:256|476:308,1,144,4|0,0:0|0:0,0:0:0:0:
396,280,6623,1,0,0:0:0:0:
324,328,6857,2,0,P|288:332|252:324,1,72
180,280,7326,2,0,L|108:284,1,72,0|0,0:0|0:0,0:0:0:0:
256,192,7795,12,0,9670,0:0:0:0:
428,212,10138,1,0,0:0:0:0:
292,320,10607,1,0,0:0:0:0:
184,184,11076,2,0,L|112:180,1,72,0|0,0:0|0:0,0:0:0:0:
24,172,11545,5,6,0:0:0:0:
160,280,12013,1,0,0:0:0:0:
268,144,12482,1,0,0:0:0:0:
132,36,12951,2,0,L|204:32,1,72,0|0,0:0|0:0,0:0:0:0:
284,60,13420,6,0,P|340:100|344:180,2,144,6|0|0,0:0|0:0|0:0,0:0:0:0:
268,144,14591,1,0,0:0:0:0:
284,228,14826,2,0,P|316:248|364:252,1,72,0|0,0:0|0:0,0:0:0:0:
436,248,15295,6,0,P|372:272|344:340,1,144,6|2,0:0|0:0,0:0:0:0:
168,338,16232,2,0,P|141:273|76:248,1,144,2|2,0:0|0:0,0:0:0:0:
4,296,16935,1,0,0:0:0:0:
80,336,17170,5,6,0:0:0:0:
44,168,17638,1,0,0:0:0:0:
212,128,18107,1,0,0:0:0:0:
248,296,18576,2,0,P|284:288|320:292,1,72,0|0,0:0|0:0,0:0:0:0:
400,324,19045,5,6,0:0:0:0:
280,200,19513,1,0,0:0:0:0:
368,52,19982,1,0,0:0:0:0:
488,176,20451,2,0,P|452:168|416:172,1,72,0|0,0:0|0:0,0:0:0:0:
336,200,20920,6,0,P|284:216|200:192,1,144,6|0,0:0|0:0,0:0:0:0:
200,192,21857,2,0,L|204:264,1,72,0|0,0:3|0:0,0:0:0:0:
117,244,22326,2,0,L|120:172,1,72,0|0,0:0|0:0,0:0:0:0:
40,152,22795,6,0,L|28:296,2,144,6|0|0,0:0|0:0|0:0,0:0:0:0:
152,24,24201,1,0,0:0:0:0:
220,76,24435,1,0,3:0:0:0:
304,56,24670,6,0,P|288:120|296:196,1,144,4|2,0:3|0:3,0:0:0:0:
344,268,25373,1,0,0:0:0:0:
416,316,25607,2,0,P|452:312|508:316,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
244,344,26545,6,0,P|176:356|108:328,1,144,4|2,0:3|0:3,0:0:0:0:
60,256,27248,1,0,0:0:0:0:
36,172,27482,2,0,L|40:100,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
188,252,28420,6,0,P|192:184|196:100,1,144,4|2,0:3|0:3,0:0:0:0:
140,40,29123,1,0,0:0:0:0:
140,40,29357,2,0,B|172:16|220:24|220:24|288:36,1,144,0|2,0:0|0:3,0:0:0:0:
364,52,30060,1,0,0:0:0:0:
308,116,30295,6,0,B|300:168|300:168|328:256,1,144,4|2,0:3|0:3,0:0:0:0:
340,340,30998,1,0,0:0:0:0:
260,308,31232,2,0,L|188:304,1,72,0|2,0:0|0:3,0:0:0:0:
100,296,31701,1,2,0:3:0:0:
136,374,31935,1,0,0:0:0:0:
152,224,32170,6,0,P|160:152|132:88,1,144,4|2,0:3|0:3,0:0:0:0:
56,48,32873,1,0,0:0:0:0:
60,136,33107,2,0,L|56:208,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
224,76,34045,6,0,P|289:104|360:96,1,144,4|2,0:3|0:3,0:0:0:0:
432,48,34748,1,0,0:0:0:0:
440,132,34982,2,0,B|432:156|432:156|436:204,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
448,304,35920,6,0,B|412:315|380:292|380:292|348:269|312:280,1,144,4|2,0:3|0:3,0:0:0:0:
332,364,36623,1,0,0:0:0:0:
247,339,36857,2,0,P|230:308|225:273,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
312,280,37560,6,0,L|316:172,1,108
134,35,38966,5,0,0:0:0:0:
72,96,39201,2,0,P|119:119|171:111,1,108,0|0,0:0|0:0,0:0:0:0:
192,100,39670,6,0,L|200:172,1,72,4|2,0:0|0:0,0:0:0:0:
147,240,40138,2,0,P|133:272|132:308,1,72,0|2,1:0|0:0,0:0:0:0:
216,292,40607,2,0,B|260:308|260:308|356:292,1,144,4|0,2:3|1:0,1:0:0:0:
356,292,41310,1,2,0:0:0:0:
436,327,41545,6,0,P|441:292|435:257,1,72,4|2,0:3|0:0,0:0:0:0:
364,204,42013,2,0,P|336:144|352:68,1,144,0|4,1:0|2:3,1:0:0:0:
404,0,42716,1,2,0:0:0:0:
440,80,42951,2,0,B|464:84|464:84|512:80,1,72,0|2,1:0|0:0,0:0:0:0:
351,71,43420,6,0,B|296:68|296:68|268:76|268:76|196:72,1,144,4|0,2:3|1:0,1:0:0:0:
120,68,44123,1,2,0:0:0:0:
160,144,44357,2,0,P|172:180|168:232,1,72,4|2,0:3|0:0,0:0:0:0:
76,264,44826,2,0,P|76:228|88:194,1,72,0|2,1:0|0:0,0:0:0:0:
160,144,45295,5,4,0:3:0:0:
244,164,45529,1,2,0:0:0:0:
268,248,45763,2,0,L|344:252,1,72,0|2,1:0|0:0,0:0:0:0:
408,156,46232,2,0,L|336:159,1,72,4|2,0:3|0:0,0:0:0:0:
212,72,46701,2,0,L|288:76,1,72,0|2,1:0|0:0,0:0:0:0:
400,72,47170,6,0,P|464:96|488:172,1,144,4|0,2:0|1:0,1:0:0:0:
476,248,47873,1,2,0:0:0:0:
436,324,48107,2,0,L|284:320,1,144,4|0,2:3|1:0,1:0:0:0:
204,316,48810,1,2,0:0:0:0:
127,355,49045,6,0,P|120:321|124:285,1,72,4|2,0:3|0:0,0:0:0:0:
192,232,49513,2,0,L|335:228,1,144,0|4,1:0|2:3,1:0:0:0:
412,188,50216,1,2,0:0:0:0:
444,108,50451,2,0,P|452:72|448:36,1,72,0|2,1:0|0:0,0:0:0:0:
368,68,50920,6,0,B|332:79|300:56|300:56|268:33|232:44,1,144,4|0,2:3|1:0,1:0:0:0:
152,76,51623,1,2,0:0:0:0:
76,116,51857,2,0,L|80:268,1,144,4|0,2:3|1:0,1:0:0:0:
80,260,52560,1,2,0:0:0:0:
8,308,52795,6,0,P|34:334|69:346,1,72,4|2,0:3|0:0,0:0:0:0:
148,312,53263,2,0,P|163:278|162:241,1,72,0|2,1:0|0:0,0:0:0:0:
156,156,53732,5,0,3:0:0:0:
156,156,53966,1,2,0:0:0:0:
236,196,54201,2,0,L|312:192,1,72,8|0,0:3|0:0,0:0:0:0:
368,256,54670,6,0,P|392:216|352:116,1,144,4|2,0:0|1:2,0:0:0:0:
288,92,55373,1,0,0:0:0:0:
360,40,55607,2,0,L|432:36,1,72,4|0,0:3|3:0,0:0:0:0:
288,92,56076,2,0,L|216:88,1,72,2|0,1:2|0:0,0:0:0:0:
132,72,56545,6,0,P|172:88|200:184,1,144,4|2,0:3|1:2,0:0:0:0:
143,241,57248,1,0,0:0:0:0:
65,202,57482,2,0,P|87:174|119:157,1,72,4|0,0:3|3:0,0:0:0:0:
132,324,57951,2,0,P|98:312|72:288,1,72,2|0,1:2|0:0,0:0:0:0:
143,241,58420,6,0,L|288:240,1,144,4|2,0:3|1:2,0:0:0:0:
372,240,59123,1,0,0:0:0:0:
330,314,59357,2,0,P|318:350|322:390,1,72,4|0,0:3|3:0,0:0:0:0:
452,264,59826,2,0,P|453:228|442:194,1,72,2|0,1:2|0:0,0:0:0:0:
384,128,60295,6,0,B|336:144|336:144|244:128,1,144,4|2,0:3|1:2,0:0:0:0:
164,160,60998,2,0,P|160:116|168:88,1,72,0|4,0:0|0:3,0:0:0:0:
244,128,61466,2,0,P|248:172|240:200,1,72,0|2,3:0|1:2,0:0:0:0:
168,248,61935,1,0,0:0:0:0:
120,320,62170,6,0,P|196:328|252:272,2,144,4|2|4,0:3|1:2|0:3,0:0:0:0:
80,244,63341,1,0,3:0:0:0:
100,160,63576,2,0,L|24:156,1,72,2|0,1:2|0:0,0:0:0:0:
180,128,64045,6,0,P|249:138|304:94,1,144,4|2,0:3|1:2,0:0:0:0:
226,57,64748,1,0,0:0:0:0:
304,94,64982,2,0,L|300:166,1,72,4|0,0:3|3:0,0:0:0:0:
377,203,65451,2,0,L|388:132,1,72,2|0,1:2|0:0,0:0:0:0:
468,180,65920,6,0,L|432:328,1,144,4|2,0:3|1:2,0:0:0:0:
276,252,66857,2,0,P|208:248|140:280,1,144,4|2,0:3|1:2,0:0:0:0:
84,344,67560,1,0,0:0:0:0:
56,260,67795,6,0,L|52:188,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
168,128,68732,2,0,L|172:56,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
244,168,69435,1,0,0:0:0:0:
332,164,69670,1,4,0:3:0:0:
208,328,84670,6,0,P|224:264|216:188,1,144,4|2,0:3|0:3,0:0:0:0:
168,116,85373,1,0,0:0:0:0:
96,68,85607,2,0,P|60:72|4:68,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
268,40,86545,6,0,P|336:28|404:56,1,144,4|2,0:3|0:3,0:0:0:0:
452,128,87248,1,0,0:0:0:0:
476,212,87482,2,0,L|472:284,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
324,132,88420,6,0,P|320:200|316:284,1,144,4|2,0:3|0:3,0:0:0:0:
372,344,89123,1,0,0:0:0:0:
372,344,89357,2,0,B|340:368|292:360|292:360|224:348,1,144,0|2,0:0|0:3,0:0:0:0:
148,332,90060,1,0,0:0:0:0:
204,268,90295,6,0,B|212:216|212:216|184:128,1,144,4|2,0:3|0:3,0:0:0:0:
172,44,90998,1,0,0:0:0:0:
252,76,91232,2,0,L|324:80,1,72,0|2,0:0|0:3,0:0:0:0:
412,88,91701,1,2,0:3:0:0:
377,9,91935,1,0,0:0:0:0:
360,160,92170,6,0,P|352:232|380:296,1,144,4|2,0:3|0:3,0:0:0:0:
456,336,92873,1,0,0:0:0:0:
452,248,93107,2,0,L|456:176,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
288,308,94045,6,0,P|223:280|152:288,1,144,4|2,0:3|0:3,0:0:0:0:
80,336,94748,1,0,0:0:0:0:
72,252,94982,2,0,B|80:228|80:228|76:180,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
64,80,95920,6,0,B|100:69|132:92|132:92|164:115|200:104,1,144,4|2,0:3|0:3,0:0:0:0:
180,20,96623,1,0,0:0:0:0:
265,45,96857,2,0,P|282:76|287:111,2,72,0|0|2,0:0|0:0|0:3,0:0:0:0:
200,104,97560,1,0,0:0:0:0:
200,104,97677,1,0,0:0:0:0:
200,104,97795,6,0,B|196:142|217:166|217:166|176:180|160:220,1,144,4|0,0:3|0:0,0:0:0:0:
240,248,98966,5,0,0:0:0:0:
202,325,99201,2,0,P|254:333|301:309,1,108,0|0,0:0|0:0,0:0:0:0:
315,292,99670,6,0,L|323:220,1,72,4|2,0:0|0:0,0:0:0:0:
365,144,100138,2,0,P|379:112|380:76,1,72,0|2,1:0|0:0,0:0:0:0:
296,92,100607,2,0,B|252:76|252:76|156:92,1,144,4|0,2:3|1:0,1:0:0:0:
156,92,101310,1,2,0:0:0:0:
76,57,101545,6,0,P|71:92|77:127,1,72,4|2,0:3|0:0,0:0:0:0:
148,180,102013,2,0,P|176:240|160:316,1,144,0|4,1:0|2:3,1:0:0:0:
108,384,102716,1,2,0:0:0:0:
72,304,102951,2,0,B|48:300|48:300|0:304,1,72,0|2,1:0|0:0,0:0:0:0:
161,313,103420,6,0,B|216:316|216:316|244:308|244:308|316:312,1,144,4|0,2:3|1:0,1:0:0:0:
392,316,104123,1,2,0:0:0:0:
352,240,104357,2,0,P|340:204|344:152,1,72,4|2,0:3|0:0,0:0:0:0:
436,120,104826,2,0,P|436:156|424:190,1,72,0|2,1:0|0:0,0:0:0:0:
352,240,105295,5,4,0:3:0:0:
268,220,105529,1,2,0:0:0:0:
244,136,105763,2,0,L|168:132,1,72,0|2,1:0|0:0,0:0:0:0:
104,228,106232,2,0,L|176:225,1,72,4|2,0:3|0:0,0:0:0:0:
300,312,106701,2,0,L|224:308,1,72,0|2,1:0|0:0,0:0:0:0:
112,312,107170,6,0,P|48:288|24:212,1,144,4|0,2:0|1:0,1:0:0:0:
36,136,107873,1,2,0:0:0:0:
76,60,108107,2,0,L|228:64,1,144,4|0,2:3|1:0,1:0:0:0:
308,68,108810,1,2,0:0:0:0:
385,29,109045,6,0,P|392:63|388:99,1,72,4|2,0:3|0:0,0:0:0:0:
320,152,109513,2,0,L|177:156,1,144,0|4,1:0|2:3,1:0:0:0:
100,196,110216,1,2,0:0:0:0:
68,276,110451,2,0,P|60:312|64:348,1,72,0|2,1:0|0:0,0:0:0:0:
144,316,110920,6,0,B|180:305|212:328|212:328|244:351|280:340,1,144,4|0,2:3|1:0,1:0:0:0:
360,308,111623,1,2,0:0:0:0:
436,268,111857,2,0,L|432:116,1,144,4|0,2:3|1:0,1:0:0:0:
432,124,112560,1,2,0:0:0:0:
504,76,112795,6,0,P|478:50|443:38,1,72,4|2,0:3|0:0,0:0:0:0:
364,72,113263,2,0,P|349:106|350:143,1,72,0|2,1:0|0:0,0:0:0:0:
356,228,113732,5,0,3:0:0:0:
356,228,113966,1,2,0:0:0:0:
276,188,114201,2,0,L|200:192,1,72,8|0,0:3|0:0,0:0:0:0:
144,128,114670,6,0,P|120:168|160:268,1,144,4|2,0:0|1:2,0:0:0:0:
224,292,115373,1,0,0:0:0:0:
152,344,115607,2,0,L|80:348,1,72,4|0,0:3|3:0,0:0:0:0:
224,292,116076,2,0,L|296:296,1,72,2|0,1:2|0:0,0:0:0:0:
380,312,116545,6,0,P|340:296|312:200,1,144,4|2,0:3|1:2,0:0:0:0:
369,143,117248,1,0,0:0:0:0:
447,182,117482,2,0,P|425:210|393:227,1,72,4|0,0:3|3:0,0:0:0:0:
380,60,117951,2,0,P|414:72|440:96,1,72,2|0,1:2|0:0,0:0:0:0:
369,143,118420,6,0,L|224:144,1,144,4|2,0:3|1:2,0:0:0:0:
140,144,119123,1,0,0:0:0:0:
182,70,119357,2,0,P|194:34|190:-6,1,72,4|0,0:3|3:0,0:0:0:0:
60,120,119826,2,0,P|59:156|70:190,1,72,2|0,1:2|0:0,0:0:0:0:
128,256,120295,6,0,B|176:240|176:240|268:256,1,144,4|2,0:3|1:2,0:0:0:0:
348,224,120998,2,0,P|352:268|344:296,1,72,0|4,0:0|0:3,0:0:0:0:
268,256,121466,2,0,P|264:212|272:184,1,72,0|2,3:0|1:2,0:0:0:0:
344,136,121935,1,0,0:0:0:0:
392,64,122170,6,0,P|316:56|260:112,2,144,4|2|4,0:3|1:2|0:3,0:0:0:0:
432,140,123341,1,0,3:0:0:0:
412,224,123576,2,0,L|488:228,1,72,2|0,1:2|0:0,0:0:0:0:
332,256,124045,6,0,P|263:246|208:290,1,144,4|2,0:3|1:2,0:0:0:0:
286,327,124748,1,0,0:0:0:0:
208,290,124982,2,0,L|212:218,1,72,4|0,0:3|3:0,0:0:0:0:
135,181,125451,2,0,L|124:252,1,72,2|0,1:2|0:0,0:0:0:0:
44,204,125920,6,0,L|80:56,1,144,4|2,0:3|1:2,0:0:0:0:
236,132,126857,2,0,P|304:136|372:104,1,144,4|2,0:3|1:2,0:0:0:0:
428,40,127560,1,0,0:0:0:0:
456,124,127795,6,0,L|460:196,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
344,256,128732,2,0,L|340:328,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
268,216,129435,1,0,0:0:0:0:
180,220,129670,5,4,2:0:0:0:
256,40,130373,1,2,0:0:0:0:
64,68,131076,1,2,0:0:0:0:
92,136,131310,1,0,0:0:0:0:
64,204,131545,6,0,L|60:288,1,72
31,343,132248,2,0,P|86:345|127:309,1,108
332,220,133420,5,2,0:0:0:0:
256,40,134123,1,2,0:0:0:0:
448,68,134826,1,2,0:0:0:0:
420,136,135060,1,0,0:0:0:0:
448,204,135295,6,0,L|452:288,1,72,2|0,0:0|0:0,0:0:0:0:
480,343,135998,2,0,P|426:345|385:309,1,108
256,192,137170,5,2,0:0:0:0:
156,360,137873,1,2,0:0:0:0:
356,360,138576,2,0,L|352:308,1,36,2|0,0:0|0:0,0:0:0:0:
304,268,139045,6,0,P|336:253|372:252,1,72
448,260,139748,2,0,L|444:152,1,108
256,192,140920,5,2,0:0:0:0:
356,24,141623,1,2,0:0:0:0:
156,24,142326,2,0,L|160:72,1,36,2|0,0:0|0:0,0:0:0:0:
208,116,142795,6,0,P|176:131|140:132,1,72,2|0,0:0|0:0,0:0:0:0:
64,124,143498,2,0,L|68:232,1,108
68,232,144670,5,4,0:3:0:0:
216,320,145138,1,4,0:3:0:0:
304,172,145607,1,4,0:3:0:0:
156,84,146075,1,4,0:3:0:0:
296,320,146545,5,4,0:3:0:0:
208,172,147013,1,4,0:3:0:0:
356,84,147482,1,4,0:3:0:0:
444,232,147950,1,4,0:3:0:0:
296,320,148420,6,0,P|252:328|192:296,2,108.000004119873,4|4|4,0:3|0:3|0:3,0:0:0:0:
260,248,149591,1,0,0:0:0:0:
320,196,149826,2,0,L|316:140,1,54.0000020599366,4|0,0:3|0:0,0:0:0:0:
120,236,159670,6,0,L|176:232,1,54.0000020599366,4|0,0:3|0:0,0:0:0:0:
160,152,160138,2,0,L|104:156,1,54.0000020599366,2|0,0:0|0:0,0:0:0:0:
240,180,160607,2,0,P|292:188|344:172,1,108.000004119873,4|2,0:3|0:0,3:0:0:0:
408,120,161310,1,0,3:0:0:0:
424,200,161545,6,0,L|420:256,1,54.0000020599366,4|0,0:3|0:0,0:0:0:0:
376,320,162013,2,0,P|396:328|480:304,2,108.000004119873,2|6|2,2:0|0:3|2:0,3:0:0:0:
312,268,163185,1,0,0:0:0:0:
296,348,163420,6,0,L|240:344,1,54.0000020599366,4|0,3:0|3:0,0:0:0:0:
160,320,163888,2,0,L|100:316,1,57.6,4|0,3:0|3:0,0:0:0:0:
64,232,164357,6,0,L|128:228,1,61.2000011672974,4|0,3:0|3:0,0:0:0:0:
204,200,164825,2,0,L|268:196,1,61.2000011672974,4|0,3:0|3:0,0:0:0:0:
232,108,165295,6,0,L|164:104,1,68.399998173523,4|0,3:0|3:0,0:0:0:0:
80,84,165763,2,0,L|4:80,1,72,4|0,3:0|3:0,0:0:0:0:
324,120,167170,6,0,P|388:128|456:92,1,144,4|2,0:0|1:2,0:0:0:0:
496,168,167873,1,0,0:0:0:0:
496,168,168107,2,0,P|484:204|488:256,1,72,4|0,0:3|3:0,0:0:0:0:
408,296,168576,2,0,P|398:261|378:231,1,72,2|0,1:2|0:0,0:0:0:0:
296,200,169045,6,0,B|228:228|156:204,1,144,4|2,0:3|1:2,0:0:0:0:
84,156,169748,1,0,0:0:0:0:
80,244,169982,2,0,L|76:316,1,72,4|0,0:3|3:0,0:0:0:0:
170,274,170451,2,0,L|156:204,1,72,2|0,1:2|0:0,0:0:0:0:
216,140,170920,6,0,L|284:276,1,144,4|2,0:3|1:2,0:0:0:0:
320,344,171623,1,0,0:0:0:0:
372,276,171857,2,0,P|366:240|349:207,1,72,4|0,0:3|3:0,0:0:0:0:
312,132,172326,2,0,L|276:60,1,72,2|0,1:2|0:0,0:0:0:0:
208,20,172795,6,0,P|272:36|348:12,1,144,4|2,0:3|1:2,0:0:0:0:
424,48,173498,2,0,L|412:132,1,72,0|4,0:0|0:3,0:0:0:0:
484,168,173966,2,0,L|472:252,1,72,0|2,3:0|1:2,0:0:0:0:
400,280,174435,1,0,0:0:0:0:
346,348,174670,6,0,P|414:363|472:324,2,144,4|2|4,0:3|1:2|0:3,0:0:0:0:
312,268,175841,1,0,3:0:0:0:
256,336,176076,2,0,L|184:332,1,72,2|0,1:2|0:0,0:0:0:0:
80,244,176545,6,0,B|140:248|140:248|164:244|164:244|223:247,1,144,4|2,0:3|1:2,0:0:0:0:
312,268,177248,1,0,0:0:0:0:
224,247,177482,2,0,P|240:215|272:187,1,72,4|0,0:3|3:0,0:0:0:0:
204,131,177951,2,0,P|233:111|275:103,1,72,2|0,1:2|0:0,0:0:0:0:
240,23,178420,6,0,B|280:15|316:35|316:35|376:71,1,144,4|2,0:3|1:2,0:0:0:0:
399,236,179357,2,0,B|359:244|323:224|323:224|263:188,1,144,4|2,0:3|1:2,0:0:0:0:
204,132,180060,1,0,0:0:0:0:
184,216,180295,6,0,L|188:288,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
120,156,180998,1,0,0:0:0:0:
56,96,181232,2,0,L|60:24,2,72,4|2|0,0:3|0:0|1:0,0:0:0:0:
36,180,181935,1,0,0:0:0:0:
100,240,182170,6,0,P|144:300|116:380,2,144,4|2|4,0:0|1:2|0:3,0:0:0:0:
60,316,183341,1,0,0:0:0:0:
220,352,183576,2,0,L|308:348,1,72,2|0,1:2|0:0,0:0:0:0:
396,264,184045,6,0,B|336:268|336:268|312:264|312:264|253:267,1,144,4|2,0:3|1:2,0:0:0:0:
253,267,184748,1,0,0:0:0:0:
268,180,184982,2,0,L|339:177,1,72,4|0,0:3|0:0,0:0:0:0:
164,280,185451,2,0,L|92:282,1,72,2|0,1:2|0:0,0:0:0:0:
52,208,185920,6,0,P|8:268|32:344,2,144,4|2|4,0:3|1:2|0:3,0:0:0:0:
140,212,187091,1,0,0:0:0:0:
92,284,187326,2,0,P|104:316|100:368,1,72,2|0,1:2|0:0,0:0:0:0:
52,208,187795,6,0,P|48:136|76:72,1,144,4|2,0:3|1:2,0:0:0:0:
160,52,188498,2,0,P|188:28|220:16,1,72,0|4,0:0|0:3,0:0:0:0:
232,100,188966,2,0,P|268:93|301:98,1,72,0|2,0:0|1:2,0:0:0:0:
372,152,189435,1,0,0:0:0:0:
420,224,189670,6,0,P|428:296|400:360,2,144,4|2|4,0:3|1:2|0:3,0:0:0:0:
372,152,190841,1,0,0:0:0:0:
392,68,191076,2,0,L|465:64,1,72,2|0,1:2|0:0,0:0:0:0:
304,92,191545,6,0,P|236:104|168:76,1,144,4|2,0:3|1:2,0:0:0:0:
108,12,192248,1,0,0:0:0:0:
168,76,192482,2,0,L|172:152,1,72,4|0,0:3|0:0,0:0:0:0:
80,136,192951,2,0,L|101:204,1,72,2|0,1:2|0:0,0:0:0:0:
12,220,193420,6,0,B|50:279|50:279|80:300|120:292,1,144,4|2,0:3|1:2,0:0:0:0:
284,232,194357,2,0,B|320:221|352:244|352:244|384:267|420:256,1,144,4|2,0:3|1:2,0:0:0:0:
488,200,195060,1,0,0:0:0:0:
507,284,195295,6,0,P|492:315|464:338,1,72,4|0,0:0|0:0,0:0:0:0:
380,356,195763,2,0,L|236:352,1,144,0|4,1:0|0:3,0:0:0:0:
152,328,196466,1,0,3:0:0:0:
64,336,196701,2,0,P|29:325|4:300,1,72,0|0,1:0|0:0,0:0:0:0:
76,252,197170,6,0,P|108:188|96:116,1,144,4|0,0:0|1:0,0:0:0:0:
36,56,197873,1,2,0:0:0:0:
120,32,198107,2,0,L|192:28,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
248,152,199045,6,0,P|280:168|304:196,1,72,4|2,0:3|0:0,0:0:0:0:
336,277,199513,2,0,P|306:296|269:303,1,72,2|0,1:2|0:0,0:0:0:0:
183,290,199982,2,0,P|180:254|193:219,2,72,4|2|0,0:3|0:0|1:0,0:0:0:0:
436,252,200920,6,0,P|404:188|416:116,1,144,4|0,0:3|1:0,0:0:0:0:
476,56,201623,1,2,0:0:0:0:
392,32,201857,2,0,L|320:28,2,72,4|0|2,0:3|0:0|1:2,0:0:0:0:
264,152,202795,6,0,P|232:168|208:196,1,72,4|2,0:3|0:0,0:0:0:0:
176,277,203263,2,0,P|205:296|242:303,1,72,2|0,1:2|0:0,0:0:0:0:
329,290,203732,2,0,P|331:254|318:219,2,72,4|2|0,0:3|0:0|1:0,0:0:0:0:
72,324,204670,6,0,B|60:272|60:272|76:180,1,144,4|0,0:0|1:0,0:0:0:0:
92,96,205373,1,2,0:0:0:0:
8,124,205607,2,0,P|5:88|14:53,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
168,192,206545,6,0,P|200:174|237:173,1,72,4|2,0:3|0:0,0:0:0:0:
320,160,207013,2,0,P|318:196|301:229,1,72,2|0,1:2|0:0,0:0:0:0:
272,307,207482,2,0,P|240:287|221:256,2,72,4|2|0,0:3|0:0|1:0,0:0:0:0:
440,324,208420,6,0,B|452:272|452:272|436:180,1,144,4|0,0:3|1:0,0:0:0:0:
420,96,209123,1,2,0:0:0:0:
504,124,209357,2,0,P|507:88|498:53,2,72,4|0|2,0:3|0:0|1:2,0:0:0:0:
344,192,210295,6,0,P|311:174|274:173,1,72,4|2,0:3|0:0,0:0:0:0:
190,156,210763,2,0,P|191:192|208:225,1,72,2|0,1:2|0:0,0:0:0:0:
288,256,211232,1,4,0:3:0:0:
132,332,211701,1,0,1:0:0:0:
28,192,212170,6,0,P|16:120|44:56,1,144,4|0,0:0|1:0,0:0:0:0:
120,16,212873,1,2,0:0:0:0:
204,32,213107,2,0,L|304:28,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
192,204,214045,6,0,P|196:240|216:272,1,72,4|2,0:3|0:0,0:0:0:0:
298,241,214513,2,0,P|327:219|345:186,1,72,6|0,1:2|0:0,0:0:0:0:
280,132,214982,2,0,P|246:117|209:118,2,72,4|2|0,0:3|0:0|1:0,0:0:0:0:
484,192,215920,6,0,P|496:120|468:56,1,144,4|0,0:3|1:0,0:0:0:0:
392,16,216623,1,2,0:0:0:0:
308,32,216857,2,0,L|208:28,2,72,4|0|2,0:3|0:0|1:2,0:0:0:0:
320,204,217795,6,0,P|316:240|296:272,1,72,4|2,0:3|0:0,0:0:0:0:
213,241,218263,2,0,P|184:219|166:186,1,72,2|0,1:2|0:0,0:0:0:0:
232,132,218732,2,0,B|260:112|300:116|300:116|384:128,1,144,4|0,0:3|1:0,0:0:0:0:
348,336,219670,6,0,B|320:356|280:352|280:352|196:340,1,144,4|0,0:0|1:0,0:0:0:0:
124,328,220373,1,2,0:0:0:0:
54,276,220607,2,0,P|41:308|39:345,2,72,4|2|2,0:3|0:0|1:2,0:0:0:0:
156,80,221545,6,0,L|251:94,1,72,4|2,0:3|0:0,0:0:0:0:
212,169,222013,2,0,L|148:160,1,64.799998022461,2|0,1:2|0:0,0:0:0:0:
140,240,222482,2,0,L|216:252,2,57.6,4|2|0,0:3|0:0|1:0,0:0:0:0:
256,192,223420,12,0,227170,0:0:0:0:
";
    }
}
